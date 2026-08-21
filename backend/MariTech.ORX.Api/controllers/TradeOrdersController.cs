using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.TradeOrders;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/trade-orders")]
public class TradeOrdersController : ControllerBase
{
    private readonly ORXDbContext _db;

    public TradeOrdersController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetTradeOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null,
        [FromQuery] string? tradeType = null,
        [FromQuery] string? originCountry = null,
        [FromQuery] string? destinationCountry = null,
        [FromQuery] string? commodity = null,
        [FromQuery] string? currency = null,
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

            var query = _db.TradeOrders
                .AsNoTracking()
                .AsQueryable();

            // ==========================================
            // SEARCH
            // ==========================================

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(t =>
                    t.TradeId.Contains(search) ||

                    (t.CustomerId != null &&
                     t.CustomerId.Contains(search)) ||

                    (t.Buyer != null &&
                     t.Buyer.Contains(search)) ||

                    (t.Seller != null &&
                     t.Seller.Contains(search)) ||

                    (t.Commodity != null &&
                     t.Commodity.Contains(search)) ||

                    (t.OriginCountry != null &&
                     t.OriginCountry.Contains(search)) ||

                    (t.DestinationCountry != null &&
                     t.DestinationCountry.Contains(search)) ||

                    (t.TradeType != null &&
                     t.TradeType.Contains(search))
                );
            }

            // ==========================================
            // STATUS FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = status.Trim();

                query = query.Where(t =>
                    t.Status != null &&
                    t.Status == status
                );
            }

            // ==========================================
            // TRADE TYPE FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(tradeType))
            {
                tradeType = tradeType.Trim();

                query = query.Where(t =>
                    t.TradeType != null &&
                    t.TradeType == tradeType
                );
            }

            // ==========================================
            // ORIGIN COUNTRY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(originCountry))
            {
                originCountry = originCountry.Trim();

                query = query.Where(t =>
                    t.OriginCountry != null &&
                    t.OriginCountry == originCountry
                );
            }

            // ==========================================
            // DESTINATION COUNTRY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(destinationCountry))
            {
                destinationCountry = destinationCountry.Trim();

                query = query.Where(t =>
                    t.DestinationCountry != null &&
                    t.DestinationCountry == destinationCountry
                );
            }

            // ==========================================
            // COMMODITY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(commodity))
            {
                commodity = commodity.Trim();

                query = query.Where(t =>
                    t.Commodity != null &&
                    t.Commodity == commodity
                );
            }

            // ==========================================
            // CURRENCY FILTER
            // ==========================================

            if (!string.IsNullOrWhiteSpace(currency))
            {
                currency = currency.Trim();

                query = query.Where(t =>
                    t.Currency != null &&
                    t.Currency == currency
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
                "tradeid" =>
                    descending
                        ? query.OrderByDescending(t => t.TradeId)
                        : query.OrderBy(t => t.TradeId),

                "tradetype" =>
                    descending
                        ? query.OrderByDescending(t => t.TradeType)
                        : query.OrderBy(t => t.TradeType),

                "commodity" =>
                    descending
                        ? query.OrderByDescending(t => t.Commodity)
                        : query.OrderBy(t => t.Commodity),

                "tradevalue" =>
                    descending
                        ? query.OrderByDescending(t => t.TradeValue)
                        : query.OrderBy(t => t.TradeValue),

                "plannedshipdate" =>
                    descending
                        ? query.OrderByDescending(t => t.PlannedShipDate)
                        : query.OrderBy(t => t.PlannedShipDate),

                "requireddeliverydate" =>
                    descending
                        ? query.OrderByDescending(t => t.RequiredDeliveryDate)
                        : query.OrderBy(t => t.RequiredDeliveryDate),

                "status" =>
                    descending
                        ? query.OrderByDescending(t => t.Status)
                        : query.OrderBy(t => t.Status),

                _ =>
                    query.OrderBy(t => t.TradeId)
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

            var tradeOrders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TradeOrderDto
                {
                    TradeId = t.TradeId,
                    CustomerId = t.CustomerId,
                    TradeType = t.TradeType,
                    OriginCountry = t.OriginCountry,
                    DestinationCountry = t.DestinationCountry,
                    Buyer = t.Buyer,
                    Seller = t.Seller,
                    Commodity = t.Commodity,
                    TradeValue = t.TradeValue,
                    Currency = t.Currency,
                    Incoterm = t.Incoterm,
                    PaymentTerms = t.PaymentTerms,
                    PlannedShipDate = t.PlannedShipDate,
                    RequiredDeliveryDate = t.RequiredDeliveryDate,
                    Status = t.Status
                })
                .ToListAsync();

            // ==========================================
            // RESPONSE
            // ==========================================

            var response = new PagedResponse<TradeOrderDto>
            {
                Data = tradeOrders,
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