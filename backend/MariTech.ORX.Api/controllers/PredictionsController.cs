using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.Predictions;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/predictions")]
public class PredictionsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public PredictionsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetPredictions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? entityType = null,
        [FromQuery] string? entityId = null,
        [FromQuery] string? predictionType = null,
        [FromQuery] decimal? minProbability = null,
        [FromQuery] decimal? maxProbability = null,
        [FromQuery] string? predictionHorizon = null,
        [FromQuery] string? modelVersion = null,
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

            var query = _db.Predictions
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(p =>
                    p.PredictionId.Contains(search) ||

                    (p.EntityType != null &&
                     p.EntityType.Contains(search)) ||

                    (p.EntityId != null &&
                     p.EntityId.Contains(search)) ||

                    (p.PredictionType != null &&
                     p.PredictionType.Contains(search)) ||

                    (p.PredictionHorizon != null &&
                     p.PredictionHorizon.Contains(search)) ||

                    (p.ModelVersion != null &&
                     p.ModelVersion.Contains(search))
                );
            }

            // ==========================================
            // ENTITY TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(entityType))
            {
                entityType = entityType.Trim();

                query = query.Where(p =>
                    p.EntityType != null &&
                    p.EntityType == entityType);
            }

            // ==========================================
            // ENTITY ID FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(entityId))
            {
                entityId = entityId.Trim();

                query = query.Where(p =>
                    p.EntityId != null &&
                    p.EntityId == entityId);
            }

            // ==========================================
            // PREDICTION TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(predictionType))
            {
                predictionType = predictionType.Trim();

                query = query.Where(p =>
                    p.PredictionType != null &&
                    p.PredictionType == predictionType);
            }

            // ==========================================
            // PROBABILITY FILTER
            // ==========================================

            if (minProbability.HasValue)
            {
                query = query.Where(p =>
                    p.Probability >= minProbability.Value);
            }

            if (maxProbability.HasValue)
            {
                query = query.Where(p =>
                    p.Probability <= maxProbability.Value);
            }

            // ==========================================
            // PREDICTION HORIZON FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(predictionHorizon))
            {
                predictionHorizon = predictionHorizon.Trim();

                query = query.Where(p =>
                    p.PredictionHorizon != null &&
                    p.PredictionHorizon == predictionHorizon);
            }

            // ==========================================
            // MODEL VERSION FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(modelVersion))
            {
                modelVersion = modelVersion.Trim();

                query = query.Where(p =>
                    p.ModelVersion != null &&
                    p.ModelVersion == modelVersion);
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
                "predictionid" =>
                    descending
                        ? query.OrderByDescending(p => p.PredictionId)
                        : query.OrderBy(p => p.PredictionId),

                "entitytype" =>
                    descending
                        ? query.OrderByDescending(p => p.EntityType)
                        : query.OrderBy(p => p.EntityType),

                "predictiontype" =>
                    descending
                        ? query.OrderByDescending(p => p.PredictionType)
                        : query.OrderBy(p => p.PredictionType),

                "predictedvalue" =>
                    descending
                        ? query.OrderByDescending(p => p.PredictedValue)
                        : query.OrderBy(p => p.PredictedValue),

                "probability" =>
                    descending
                        ? query.OrderByDescending(p => p.Probability)
                        : query.OrderBy(p => p.Probability),

                "predictionhorizon" =>
                    descending
                        ? query.OrderByDescending(p => p.PredictionHorizon)
                        : query.OrderBy(p => p.PredictionHorizon),

                "generatedat" =>
                    descending
                        ? query.OrderByDescending(p => p.GeneratedAt)
                        : query.OrderBy(p => p.GeneratedAt),

                "expiresat" =>
                    descending
                        ? query.OrderByDescending(p => p.ExpiresAt)
                        : query.OrderBy(p => p.ExpiresAt),

                _ =>
                    query.OrderByDescending(p => p.GeneratedAt)
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

            var predictions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PredictionDto
                {
                    PredictionId = p.PredictionId,
                    EntityType = p.EntityType,
                    EntityId = p.EntityId,
                    PredictionType = p.PredictionType,
                    PredictedValue = p.PredictedValue,
                    Probability = p.Probability,
                    PredictionHorizon = p.PredictionHorizon,
                    ModelVersion = p.ModelVersion,
                    GeneratedAt = p.GeneratedAt,
                    ExpiresAt = p.ExpiresAt
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<PredictionDto>
            {
                Data = predictions,
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