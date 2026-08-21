using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.RiskAssessments;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/risk-assessments")]
public class RiskAssessmentsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public RiskAssessmentsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetRiskAssessments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? riskType = null,
        [FromQuery] string? severity = null,
        [FromQuery] string? tradeId = null,
        [FromQuery] string? shipmentId = null,
        [FromQuery] decimal? minScore = null,
        [FromQuery] decimal? maxScore = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "desc")
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

            var query = _db.RiskAssessments
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(r =>
                    r.RiskId.Contains(search) ||

                    r.TradeId.Contains(search) ||

                    r.ShipmentId.Contains(search) ||

                    (r.RiskType != null &&
                     r.RiskType.Contains(search)) ||

                    (r.Severity != null &&
                     r.Severity.Contains(search)) ||

                    (r.Reason != null &&
                     r.Reason.Contains(search)) ||

                    (r.Evidence != null &&
                     r.Evidence.Contains(search))
                );
            }

            // ==========================================
            // RISK TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(riskType))
            {
                riskType = riskType.Trim();

                query = query.Where(r =>
                    r.RiskType != null &&
                    r.RiskType == riskType);
            }

            // ==========================================
            // SEVERITY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(severity))
            {
                severity = severity.Trim();

                query = query.Where(r =>
                    r.Severity != null &&
                    r.Severity == severity);
            }

            // ==========================================
            // TRADE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(tradeId))
            {
                tradeId = tradeId.Trim();

                query = query.Where(r =>
                    r.TradeId == tradeId);
            }

            // ==========================================
            // SHIPMENT FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(shipmentId))
            {
                shipmentId = shipmentId.Trim();

                query = query.Where(r =>
                    r.ShipmentId == shipmentId);
            }

            // ==========================================
            // SCORE FILTER
            // ==========================================

            if (minScore.HasValue)
            {
                query = query.Where(r =>
                    r.Score >= minScore.Value);
            }

            if (maxScore.HasValue)
            {
                query = query.Where(r =>
                    r.Score <= maxScore.Value);
            }

            // ==========================================
            // SORTING
            // ==========================================

            bool descending =
                string.Equals(
                    sortOrder,
                    "desc",
                    StringComparison.OrdinalIgnoreCase);

            query = sortBy?.ToLower() switch
            {
                "riskid" =>
                    descending
                        ? query.OrderByDescending(r => r.RiskId)
                        : query.OrderBy(r => r.RiskId),

                "risktype" =>
                    descending
                        ? query.OrderByDescending(r => r.RiskType)
                        : query.OrderBy(r => r.RiskType),

                "score" =>
                    descending
                        ? query.OrderByDescending(r => r.Score)
                        : query.OrderBy(r => r.Score),

                "probability" =>
                    descending
                        ? query.OrderByDescending(r => r.Probability)
                        : query.OrderBy(r => r.Probability),

                "severity" =>
                    descending
                        ? query.OrderByDescending(r => r.Severity)
                        : query.OrderBy(r => r.Severity),

                "calculatedat" =>
                    descending
                        ? query.OrderByDescending(r => r.CalculatedAt)
                        : query.OrderBy(r => r.CalculatedAt),

                "expiresat" =>
                    descending
                        ? query.OrderByDescending(r => r.ExpiresAt)
                        : query.OrderBy(r => r.ExpiresAt),

                _ =>
                    query.OrderByDescending(r => r.Score)
            };

            // ==========================================
            // TOTAL COUNT
            // ==========================================

            var totalRecords = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalRecords / (double)pageSize);

            // ==========================================
            // PAGINATION + DTO
            // ==========================================

            var risks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RiskAssessmentDto
                {
                    RiskId = r.RiskId,
                    TradeId = r.TradeId,
                    ShipmentId = r.ShipmentId,
                    RiskType = r.RiskType,
                    Score = r.Score,
                    Probability = r.Probability,
                    Severity = r.Severity,
                    Reason = r.Reason,
                    Evidence = r.Evidence,
                    CalculatedAt = r.CalculatedAt,
                    ExpiresAt = r.ExpiresAt
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<RiskAssessmentDto>
            {
                Data = risks,
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