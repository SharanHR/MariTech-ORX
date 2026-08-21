using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.NetworkEvents;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/network-events")]
public class NetworkEventsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public NetworkEventsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetNetworkEvents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? eventType = null,
        [FromQuery] string? severity = null,
        [FromQuery] string? location = null,
        [FromQuery] string? carrier = null,
        [FromQuery] string? vessel = null,
        [FromQuery] string? tradeLane = null,
        [FromQuery] string? source = null,
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

            var query = _db.NetworkEvents
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(e =>
                    e.EventId.Contains(search) ||

                    (e.EventType != null &&
                     e.EventType.Contains(search)) ||

                    (e.Severity != null &&
                     e.Severity.Contains(search)) ||

                    (e.Location != null &&
                     e.Location.Contains(search)) ||

                    (e.Carrier != null &&
                     e.Carrier.Contains(search)) ||

                    (e.Vessel != null &&
                     e.Vessel.Contains(search)) ||

                    (e.TradeLane != null &&
                     e.TradeLane.Contains(search)) ||

                    (e.Description != null &&
                     e.Description.Contains(search)) ||

                    (e.Source != null &&
                     e.Source.Contains(search))
                );
            }

            // ==========================================
            // EVENT TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(eventType))
            {
                eventType = eventType.Trim();

                query = query.Where(e =>
                    e.EventType != null &&
                    e.EventType == eventType);
            }

            // ==========================================
            // SEVERITY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(severity))
            {
                severity = severity.Trim();

                query = query.Where(e =>
                    e.Severity != null &&
                    e.Severity == severity);
            }

            // ==========================================
            // LOCATION FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(location))
            {
                location = location.Trim();

                query = query.Where(e =>
                    e.Location != null &&
                    e.Location == location);
            }

            // ==========================================
            // CARRIER FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(carrier))
            {
                carrier = carrier.Trim();

                query = query.Where(e =>
                    e.Carrier != null &&
                    e.Carrier == carrier);
            }

            // ==========================================
            // VESSEL FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(vessel))
            {
                vessel = vessel.Trim();

                query = query.Where(e =>
                    e.Vessel != null &&
                    e.Vessel == vessel);
            }

            // ==========================================
            // TRADE LANE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(tradeLane))
            {
                tradeLane = tradeLane.Trim();

                query = query.Where(e =>
                    e.TradeLane != null &&
                    e.TradeLane == tradeLane);
            }

            // ==========================================
            // SOURCE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(source))
            {
                source = source.Trim();

                query = query.Where(e =>
                    e.Source != null &&
                    e.Source == source);
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
                "eventid" =>
                    descending
                        ? query.OrderByDescending(e => e.EventId)
                        : query.OrderBy(e => e.EventId),

                "eventtype" =>
                    descending
                        ? query.OrderByDescending(e => e.EventType)
                        : query.OrderBy(e => e.EventType),

                "severity" =>
                    descending
                        ? query.OrderByDescending(e => e.Severity)
                        : query.OrderBy(e => e.Severity),

                "location" =>
                    descending
                        ? query.OrderByDescending(e => e.Location)
                        : query.OrderBy(e => e.Location),

                "carrier" =>
                    descending
                        ? query.OrderByDescending(e => e.Carrier)
                        : query.OrderBy(e => e.Carrier),

                "vessel" =>
                    descending
                        ? query.OrderByDescending(e => e.Vessel)
                        : query.OrderBy(e => e.Vessel),

                "tradelane" =>
                    descending
                        ? query.OrderByDescending(e => e.TradeLane)
                        : query.OrderBy(e => e.TradeLane),

                "starttime" =>
                    descending
                        ? query.OrderByDescending(e => e.StartTime)
                        : query.OrderBy(e => e.StartTime),

                "endtime" =>
                    descending
                        ? query.OrderByDescending(e => e.EndTime)
                        : query.OrderBy(e => e.EndTime),

                "confidence" =>
                    descending
                        ? query.OrderByDescending(e => e.Confidence)
                        : query.OrderBy(e => e.Confidence),

                _ =>
                    query.OrderByDescending(e => e.StartTime)
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

            var events = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new NetworkEventDto
                {
                    EventId = e.EventId,
                    EventType = e.EventType,
                    Severity = e.Severity,
                    Location = e.Location,
                    Carrier = e.Carrier,
                    Vessel = e.Vessel,
                    TradeLane = e.TradeLane,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Description = e.Description,
                    Source = e.Source,
                    Confidence = e.Confidence
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<NetworkEventDto>
            {
                Data = events,
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