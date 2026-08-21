using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.FreightQuotes;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/freight-quotes")]
public class FreightQuotesController : ControllerBase
{
    private readonly ORXDbContext _db;

    public FreightQuotesController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetFreightQuotes(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? origin = null,
        [FromQuery] string? destination = null,
        [FromQuery] string? carrier = null,
        [FromQuery] string? containerType = null,
        [FromQuery] string? currency = null,
        [FromQuery] string? source = null,
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

            var query = _db.FreightQuotes
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(q =>
                    q.QuoteId.Contains(search) ||

                    (q.CustomerId != null &&
                     q.CustomerId.Contains(search)) ||

                    (q.Origin != null &&
                     q.Origin.Contains(search)) ||

                    (q.Destination != null &&
                     q.Destination.Contains(search)) ||

                    (q.Carrier != null &&
                     q.Carrier.Contains(search)) ||

                    (q.ContainerType != null &&
                     q.ContainerType.Contains(search)) ||

                    (q.Currency != null &&
                     q.Currency.Contains(search)) ||

                    (q.Source != null &&
                     q.Source.Contains(search))
                );
            }

            // ==========================================
            // ORIGIN FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(origin))
            {
                origin = origin.Trim();

                query = query.Where(q =>
                    q.Origin != null &&
                    q.Origin == origin
                );
            }

            // ==========================================
            // DESTINATION FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(destination))
            {
                destination = destination.Trim();

                query = query.Where(q =>
                    q.Destination != null &&
                    q.Destination == destination
                );
            }

            // ==========================================
            // CARRIER FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(carrier))
            {
                carrier = carrier.Trim();

                query = query.Where(q =>
                    q.Carrier != null &&
                    q.Carrier == carrier
                );
            }

            // ==========================================
            // CONTAINER TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(containerType))
            {
                containerType = containerType.Trim();

                query = query.Where(q =>
                    q.ContainerType != null &&
                    q.ContainerType == containerType
                );
            }

            // ==========================================
            // CURRENCY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(currency))
            {
                currency = currency.Trim();

                query = query.Where(q =>
                    q.Currency != null &&
                    q.Currency == currency
                );
            }

            // ==========================================
            // SOURCE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(source))
            {
                source = source.Trim();

                query = query.Where(q =>
                    q.Source != null &&
                    q.Source == source
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
                "quoteid" =>
                    descending
                        ? query.OrderByDescending(q => q.QuoteId)
                        : query.OrderBy(q => q.QuoteId),

                "carrier" =>
                    descending
                        ? query.OrderByDescending(q => q.Carrier)
                        : query.OrderBy(q => q.Carrier),

                "origin" =>
                    descending
                        ? query.OrderByDescending(q => q.Origin)
                        : query.OrderBy(q => q.Origin),

                "destination" =>
                    descending
                        ? query.OrderByDescending(q => q.Destination)
                        : query.OrderBy(q => q.Destination),

                "containerType" =>
                    descending
                        ? query.OrderByDescending(q => q.ContainerType)
                        : query.OrderBy(q => q.ContainerType),

                "equipmentquantity" =>
                    descending
                        ? query.OrderByDescending(q => q.EquipmentQuantity)
                        : query.OrderBy(q => q.EquipmentQuantity),

                "basefreight" =>
                    descending
                        ? query.OrderByDescending(q => q.BaseFreight)
                        : query.OrderBy(q => q.BaseFreight),

                "totalcost" =>
                    descending
                        ? query.OrderByDescending(q => q.TotalCost)
                        : query.OrderBy(q => q.TotalCost),

                "validfrom" =>
                    descending
                        ? query.OrderByDescending(q => q.ValidFrom)
                        : query.OrderBy(q => q.ValidFrom),

                "validuntil" =>
                    descending
                        ? query.OrderByDescending(q => q.ValidUntil)
                        : query.OrderBy(q => q.ValidUntil),

                _ =>
                    query.OrderBy(q => q.QuoteId)
            };

            // ==========================================
            // TOTAL COUNT
            // ==========================================

            var totalRecords = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalRecords / (double)pageSize
            );

            // ==========================================
            // PAGINATION + DTO PROJECTION
            // ==========================================

            var quotes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new FreightQuoteDto
                {
                    QuoteId = q.QuoteId,
                    CustomerId = q.CustomerId,
                    Origin = q.Origin,
                    Destination = q.Destination,
                    Carrier = q.Carrier,
                    ContainerType = q.ContainerType,
                    EquipmentQuantity = q.EquipmentQuantity,
                    BaseFreight = q.BaseFreight,
                    Baf = q.Baf,
                    LocalCharges = q.LocalCharges,
                    CongestionSurcharge = q.CongestionSurcharge,
                    OtherSurcharges = q.OtherSurcharges,
                    TotalCost = q.TotalCost,
                    Currency = q.Currency,
                    ValidFrom = q.ValidFrom,
                    ValidUntil = q.ValidUntil,
                    Source = q.Source
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<FreightQuoteDto>
            {
                Data = quotes,
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