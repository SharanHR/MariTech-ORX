using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.Documents;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/documents")]
public class DocumentsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public DocumentsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetDocuments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? documentType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? issuer = null,
        [FromQuery] string? tradeId = null,
        [FromQuery] string? shipmentId = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "asc")
    {
        try
        {
            // ==========================================
            // PAGINATION VALIDATION
            // ==========================================

            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 20;

            if (pageSize > 100)
                pageSize = 100;

            // ==========================================
            // BASE QUERY
            // ==========================================

            var query = _db.Documents
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(d =>
                    d.DocumentId.Contains(search) ||

                    d.TradeId.Contains(search) ||

                    d.ShipmentId.Contains(search) ||

                    (d.DocumentType != null &&
                     d.DocumentType.Contains(search)) ||

                    (d.DocumentNumber != null &&
                     d.DocumentNumber.Contains(search)) ||

                    (d.Issuer != null &&
                     d.Issuer.Contains(search))
                );
            }

            // ==========================================
            // DOCUMENT TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(documentType))
            {
                documentType = documentType.Trim();

                query = query.Where(d =>
                    d.DocumentType != null &&
                    d.DocumentType == documentType
                );
            }

            // ==========================================
            // STATUS FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = status.Trim();

                query = query.Where(d =>
                    d.Status != null &&
                    d.Status == status
                );
            }

            // ==========================================
            // ISSUER FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(issuer))
            {
                issuer = issuer.Trim();

                query = query.Where(d =>
                    d.Issuer != null &&
                    d.Issuer == issuer
                );
            }

            // ==========================================
            // TRADE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(tradeId))
            {
                tradeId = tradeId.Trim();

                query = query.Where(d =>
                    d.TradeId == tradeId
                );
            }

            // ==========================================
            // SHIPMENT FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(shipmentId))
            {
                shipmentId = shipmentId.Trim();

                query = query.Where(d =>
                    d.ShipmentId == shipmentId
                );
            }

            // ==========================================
            // SORTING
            // ==========================================

            bool descending =
                string.Equals(
                    sortOrder,
                    "desc",
                    StringComparison.OrdinalIgnoreCase
                );

            query = sortBy?.ToLower() switch
            {
                "documentid" =>
                    descending
                        ? query.OrderByDescending(d => d.DocumentId)
                        : query.OrderBy(d => d.DocumentId),

                "documenttype" =>
                    descending
                        ? query.OrderByDescending(d => d.DocumentType)
                        : query.OrderBy(d => d.DocumentType),

                "documentnumber" =>
                    descending
                        ? query.OrderByDescending(d => d.DocumentNumber)
                        : query.OrderBy(d => d.DocumentNumber),

                "issuer" =>
                    descending
                        ? query.OrderByDescending(d => d.Issuer)
                        : query.OrderBy(d => d.Issuer),

                "issuedate" =>
                    descending
                        ? query.OrderByDescending(d => d.IssueDate)
                        : query.OrderBy(d => d.IssueDate),

                "status" =>
                    descending
                        ? query.OrderByDescending(d => d.Status)
                        : query.OrderBy(d => d.Status),

                "version" =>
                    descending
                        ? query.OrderByDescending(d => d.Version)
                        : query.OrderBy(d => d.Version),

                _ =>
                    query.OrderBy(d => d.DocumentId)
            };

            // ==========================================
            // TOTAL COUNT
            // ==========================================

            var totalRecords = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalRecords / (double)pageSize
            );

            // ==========================================
            // PAGINATION + DTO
            // ==========================================

            var documents = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DocumentDto
                {
                    DocumentId = d.DocumentId,
                    TradeId = d.TradeId,
                    ShipmentId = d.ShipmentId,
                    DocumentType = d.DocumentType,
                    DocumentNumber = d.DocumentNumber,
                    Issuer = d.Issuer,
                    IssueDate = d.IssueDate,
                    Status = d.Status,
                    Version = d.Version
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<DocumentDto>
            {
                Data = documents,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "error",
                message = ex.Message
            });
        }
    }
}