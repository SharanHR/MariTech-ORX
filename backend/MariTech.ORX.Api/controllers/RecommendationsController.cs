using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.Recommendations;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/recommendations")]
public class RecommendationsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public RecommendationsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecommendations(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? tradeId = null,
        [FromQuery] string? recommendationType = null,
        [FromQuery] string? status = null,
        [FromQuery] decimal? minConfidence = null,
        [FromQuery] decimal? maxConfidence = null,
        [FromQuery] decimal? minRiskReduction = null,
        [FromQuery] decimal? maxRiskReduction = null,
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

            var query = _db.Recommendations
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(r =>
                    r.RecommendationId.Contains(search) ||

                    r.TradeId.Contains(search) ||

                    (r.TriggerEvent != null &&
                     r.TriggerEvent.Contains(search)) ||

                    (r.RecommendationType != null &&
                     r.RecommendationType.Contains(search)) ||

                    (r.Action != null &&
                     r.Action.Contains(search)) ||

                    (r.Reason != null &&
                     r.Reason.Contains(search)) ||

                    (r.ExpectedBenefit != null &&
                     r.ExpectedBenefit.Contains(search))
                );
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
            // RECOMMENDATION TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(recommendationType))
            {
                recommendationType = recommendationType.Trim();

                query = query.Where(r =>
                    r.RecommendationType != null &&
                    r.RecommendationType == recommendationType);
            }

            // ==========================================
            // STATUS FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = status.Trim();

                query = query.Where(r =>
                    r.Status != null &&
                    r.Status == status);
            }

            // ==========================================
            // CONFIDENCE FILTER
            // ==========================================

            if (minConfidence.HasValue)
            {
                query = query.Where(r =>
                    r.Confidence >= minConfidence.Value);
            }

            if (maxConfidence.HasValue)
            {
                query = query.Where(r =>
                    r.Confidence <= maxConfidence.Value);
            }

            // ==========================================
            // RISK REDUCTION FILTER
            // ==========================================

            if (minRiskReduction.HasValue)
            {
                query = query.Where(r =>
                    r.RiskReduction >= minRiskReduction.Value);
            }

            if (maxRiskReduction.HasValue)
            {
                query = query.Where(r =>
                    r.RiskReduction <= maxRiskReduction.Value);
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
                "recommendationid" =>
                    descending
                        ? query.OrderByDescending(r => r.RecommendationId)
                        : query.OrderBy(r => r.RecommendationId),

                "tradeid" =>
                    descending
                        ? query.OrderByDescending(r => r.TradeId)
                        : query.OrderBy(r => r.TradeId),

                "recommendationtype" =>
                    descending
                        ? query.OrderByDescending(r => r.RecommendationType)
                        : query.OrderBy(r => r.RecommendationType),

                "estimatedcost" =>
                    descending
                        ? query.OrderByDescending(r => r.EstimatedCost)
                        : query.OrderBy(r => r.EstimatedCost),

                "riskreduction" =>
                    descending
                        ? query.OrderByDescending(r => r.RiskReduction)
                        : query.OrderBy(r => r.RiskReduction),

                "confidence" =>
                    descending
                        ? query.OrderByDescending(r => r.Confidence)
                        : query.OrderBy(r => r.Confidence),

                "status" =>
                    descending
                        ? query.OrderByDescending(r => r.Status)
                        : query.OrderBy(r => r.Status),

                _ =>
                    query.OrderByDescending(r => r.Confidence)
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

            var recommendations = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecommendationDto
                {
                    RecommendationId = r.RecommendationId,
                    TradeId = r.TradeId,
                    TriggerEvent = r.TriggerEvent,
                    RecommendationType = r.RecommendationType,
                    Action = r.Action,
                    Reason = r.Reason,
                    ExpectedBenefit = r.ExpectedBenefit,
                    EstimatedCost = r.EstimatedCost,
                    RiskReduction = r.RiskReduction,
                    Confidence = r.Confidence,
                    Status = r.Status
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<RecommendationDto>
            {
                Data = recommendations,
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