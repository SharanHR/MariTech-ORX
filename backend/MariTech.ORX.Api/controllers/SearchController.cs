using MariTech.ORX.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/search")]
public class SearchController : ControllerBase
{
    private readonly ORXDbContext _db;

    public SearchController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? q = null, [FromQuery] int limit = 8)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
            return Ok(Array.Empty<object>());

        limit = Math.Clamp(limit, 1, 20);
        var term = q.Trim();

        var shipments = await _db.Shipments.AsNoTracking()
            .Where(s =>
                (s.ShipmentId != null && s.ShipmentId.Contains(term)) ||
                (s.Origin != null && s.Origin.Contains(term)) ||
                (s.Destination != null && s.Destination.Contains(term)) ||
                (s.BookingReference != null && s.BookingReference.Contains(term)) ||
                (s.Carrier != null && s.Carrier.Contains(term)))
            .OrderBy(s => s.ShipmentId)
            .Take(limit)
            .Select(s => new
            {
                type = "Shipment",
                id = s.ShipmentId,
                title = $"{s.ShipmentId} · {s.Origin} → {s.Destination}",
                subtitle = s.Status ?? "Unknown",
                href = $"/shipments/{s.ShipmentId}"
            })
            .ToListAsync();

        var orders = await _db.TradeOrders.AsNoTracking()
            .Where(t =>
                (t.TradeId != null && t.TradeId.Contains(term)) ||
                (t.Buyer != null && t.Buyer.Contains(term)) ||
                (t.Seller != null && t.Seller.Contains(term)) ||
                (t.Commodity != null && t.Commodity.Contains(term)))
            .OrderBy(t => t.TradeId)
            .Take(limit)
            .Select(t => new
            {
                type = "Order",
                id = t.TradeId,
                title = $"{t.TradeId} · {t.Commodity}",
                subtitle = t.Buyer ?? t.Status ?? "Trade order",
                href = "/orders"
            })
            .ToListAsync();

        var documents = await _db.Documents.AsNoTracking()
            .Where(d =>
                (d.DocumentId != null && d.DocumentId.Contains(term)) ||
                (d.DocumentNumber != null && d.DocumentNumber.Contains(term)) ||
                (d.DocumentType != null && d.DocumentType.Contains(term)) ||
                (d.ShipmentId != null && d.ShipmentId.Contains(term)))
            .OrderBy(d => d.DocumentId)
            .Take(limit)
            .Select(d => new
            {
                type = "Document",
                id = d.DocumentId,
                title = $"{d.DocumentType} · {d.ShipmentId}",
                subtitle = d.Status ?? "Document",
                href = "/documents"
            })
            .ToListAsync();

        var containers = await _db.Containers.AsNoTracking()
            .Where(c =>
                (c.ContainerNumber != null && c.ContainerNumber.Contains(term)) ||
                (c.ContainerId != null && c.ContainerId.Contains(term)) ||
                (c.ShipmentId != null && c.ShipmentId.Contains(term)) ||
                (c.CurrentLocation != null && c.CurrentLocation.Contains(term)))
            .OrderBy(c => c.ContainerId)
            .Take(limit)
            .Select(c => new
            {
                type = "Container",
                id = c.ContainerId,
                title = $"{c.ContainerNumber} · {c.ShipmentId}",
                subtitle = c.CurrentLocation ?? c.Status ?? "Container",
                href = $"/shipments/{c.ShipmentId}"
            })
            .ToListAsync();

        var results = shipments.Cast<object>()
            .Concat(orders)
            .Concat(documents)
            .Concat(containers)
            .Take(limit)
            .ToList();

        return Ok(results);
    }
}
