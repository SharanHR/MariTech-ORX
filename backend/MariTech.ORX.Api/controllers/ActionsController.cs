using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.Actions;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/actions")]
public class ActionsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public ActionsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetActions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? recommendationId = null,
        [FromQuery] string? actor = null,
        [FromQuery] string? actionType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? target = null,
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

            var query = _db.Actions
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(a =>
                    a.ActionId.Contains(search) ||

                    a.RecommendationId.Contains(search) ||

                    (a.Actor != null &&
                     a.Actor.Contains(search)) ||

                    (a.ActionType != null &&
                     a.ActionType.Contains(search)) ||

                    (a.Target != null &&
                     a.Target.Contains(search)) ||

                    (a.Status != null &&
                     a.Status.Contains(search))
                );
            }

            // ==========================================
            // RECOMMENDATION FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(recommendationId))
            {
                recommendationId = recommendationId.Trim();

                query = query.Where(a =>
                    a.RecommendationId == recommendationId);
            }

            // ==========================================
            // ACTOR FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(actor))
            {
                actor = actor.Trim();

                query = query.Where(a =>
                    a.Actor != null &&
                    a.Actor == actor);
            }

            // ==========================================
            // ACTION TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(actionType))
            {
                actionType = actionType.Trim();

                query = query.Where(a =>
                    a.ActionType != null &&
                    a.ActionType == actionType);
            }

            // ==========================================
            // STATUS FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = status.Trim();

                query = query.Where(a =>
                    a.Status != null &&
                    a.Status == status);
            }

            // ==========================================
            // TARGET FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(target))
            {
                target = target.Trim();

                query = query.Where(a =>
                    a.Target != null &&
                    a.Target.Contains(target));
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
                "actionid" =>
                    descending
                        ? query.OrderByDescending(a => a.ActionId)
                        : query.OrderBy(a => a.ActionId),

                "recommendationid" =>
                    descending
                        ? query.OrderByDescending(a => a.RecommendationId)
                        : query.OrderBy(a => a.RecommendationId),

                "actor" =>
                    descending
                        ? query.OrderByDescending(a => a.Actor)
                        : query.OrderBy(a => a.Actor),

                "actiontype" =>
                    descending
                        ? query.OrderByDescending(a => a.ActionType)
                        : query.OrderBy(a => a.ActionType),

                "status" =>
                    descending
                        ? query.OrderByDescending(a => a.Status)
                        : query.OrderBy(a => a.Status),

                "requestedat" =>
                    descending
                        ? query.OrderByDescending(a => a.RequestedAt)
                        : query.OrderBy(a => a.RequestedAt),

                _ =>
                    query.OrderByDescending(a => a.RequestedAt)
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

            var actions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ActionDto
                {
                    ActionId = a.ActionId,
                    RecommendationId = a.RecommendationId,
                    Actor = a.Actor,
                    ActionType = a.ActionType,
                    Target = a.Target,
                    Status = a.Status,
                    RequestedAt = a.RequestedAt
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<ActionDto>
            {
                Data = actions,
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