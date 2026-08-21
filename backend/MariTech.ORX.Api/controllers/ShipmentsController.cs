using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using MariTech.ORX.Api.Models.Common;
using MariTech.ORX.Api.Models.Shipments;
using MariTech.ORX.Infrastructure.Persistence;
using MariTech.ORX.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/shipments")]
public class ShipmentsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public ShipmentsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetShipments(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null,
        [FromQuery] string? origin = null,
        [FromQuery] string? destination = null,
        [FromQuery] string? carrier = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = "asc")
    {
        try
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100);

            var query = _db.Shipments.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(s =>
                    (s.ShipmentId != null && s.ShipmentId.Contains(search)) ||
                    (s.TradeId != null && s.TradeId.Contains(search)) ||
                    (s.Shipper != null && s.Shipper.Contains(search)) ||
                    (s.Consignee != null && s.Consignee.Contains(search)) ||
                    (s.Forwarder != null && s.Forwarder.Contains(search)) ||
                    (s.Carrier != null && s.Carrier.Contains(search)) ||
                    (s.BookingReference != null && s.BookingReference.Contains(search)) ||
                    (s.Origin != null && s.Origin.Contains(search)) ||
                    (s.Destination != null && s.Destination.Contains(search)) ||
                    (s.PortOfLoading != null && s.PortOfLoading.Contains(search)) ||
                    (s.PortOfDischarge != null && s.PortOfDischarge.Contains(search))
                );
            }

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(s => s.Status != null && s.Status == status.Trim());

            if (!string.IsNullOrWhiteSpace(origin))
                query = query.Where(s => s.Origin != null && s.Origin == origin.Trim());

            if (!string.IsNullOrWhiteSpace(destination))
                query = query.Where(s => s.Destination != null && s.Destination == destination.Trim());

            if (!string.IsNullOrWhiteSpace(carrier))
                query = query.Where(s => s.Carrier != null && s.Carrier == carrier.Trim());

            bool descending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.ToLowerInvariant() switch
            {
                "shipmentid" => descending ? query.OrderByDescending(s => s.ShipmentId) : query.OrderBy(s => s.ShipmentId),
                "status" => descending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
                "origin" => descending ? query.OrderByDescending(s => s.Origin) : query.OrderBy(s => s.Origin),
                "destination" => descending ? query.OrderByDescending(s => s.Destination) : query.OrderBy(s => s.Destination),
                "carrier" => descending ? query.OrderByDescending(s => s.Carrier) : query.OrderBy(s => s.Carrier),
                "plannedetd" => descending ? query.OrderByDescending(s => s.PlannedEtd) : query.OrderBy(s => s.PlannedEtd),
                "plannedeta" => descending ? query.OrderByDescending(s => s.PlannedEta) : query.OrderBy(s => s.PlannedEta),
                "currentetd" => descending ? query.OrderByDescending(s => s.CurrentEtd) : query.OrderBy(s => s.CurrentEtd),
                "currenteta" => descending ? query.OrderByDescending(s => s.CurrentEta) : query.OrderBy(s => s.CurrentEta),
                "riskscore" => descending ? query.OrderByDescending(s => s.RiskScore) : query.OrderBy(s => s.RiskScore),
                _ => query.OrderBy(s => s.ShipmentId)
            };

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var shipments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new ShipmentDto
                {
                    ShipmentId = s.ShipmentId,
                    TradeId = s.TradeId,
                    Shipper = s.Shipper,
                    Consignee = s.Consignee,
                    Forwarder = s.Forwarder,
                    Carrier = s.Carrier,
                    BookingReference = s.BookingReference,
                    Origin = s.Origin,
                    Destination = s.Destination,
                    PortOfLoading = s.PortOfLoading,
                    PortOfDischarge = s.PortOfDischarge,
                    PlannedEtd = s.PlannedEtd,
                    PlannedEta = s.PlannedEta,
                    CurrentEtd = s.CurrentEtd,
                    CurrentEta = s.CurrentEta,
                    Status = s.Status,
                    RiskScore = s.RiskScore
                })
                .ToListAsync();

            return Ok(new PagedResponse<ShipmentDto>
            {
                Data = shipments,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "error", message = ex.Message });
        }
    }

    [HttpGet("{shipmentId}")]
    public async Task<IActionResult> GetShipment(string shipmentId)
    {
        var shipment = await _db.Shipments
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ShipmentId == shipmentId);

        if (shipment == null)
            return NotFound(new { message = "Shipment not found." });

        var trade = await _db.TradeOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TradeId == shipment.TradeId);

        var cargo = await _db.Cargos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.TradeId == shipment.TradeId);

        var workflow = await _db.ShipmentWorkflowDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShipmentId == shipment.ShipmentId);

        var documents = await _db.Documents
            .AsNoTracking()
            .Where(d => d.ShipmentId == shipment.ShipmentId)
            .Select(d => new
            {
                d.DocumentId,
                d.DocumentType,
                d.Status,
                d.Version
            })
            .ToListAsync();

        return Ok(new
        {
            shipment = new ShipmentDto
            {
                ShipmentId = shipment.ShipmentId,
                TradeId = shipment.TradeId,
                Shipper = shipment.Shipper,
                Consignee = shipment.Consignee,
                Forwarder = shipment.Forwarder,
                Carrier = shipment.Carrier,
                BookingReference = shipment.BookingReference,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                PortOfLoading = shipment.PortOfLoading,
                PortOfDischarge = shipment.PortOfDischarge,
                PlannedEtd = shipment.PlannedEtd,
                PlannedEta = shipment.PlannedEta,
                CurrentEtd = shipment.CurrentEtd,
                CurrentEta = shipment.CurrentEta,
                Status = shipment.Status,
                RiskScore = shipment.RiskScore
            },
            trade = trade == null ? null : new
            {
                trade.TradeId,
                trade.Buyer,
                trade.Seller,
                trade.Commodity,
                trade.TradeValue,
                trade.Currency,
                trade.Incoterm,
                trade.RequiredDeliveryDate,
                trade.Status
            },
            cargo = cargo == null ? null : new
            {
                cargo.CargoId,
                cargo.Commodity,
                cargo.HsCode,
                cargo.Quantity,
                cargo.QuantityUnit,
                cargo.GrossWeight,
                cargo.Volume,
                cargo.DangerousGoods,
                cargo.ContainerRequirement
            },
            workflow = workflow == null ? null : new
            {
                workflow.ShipmentMode,
                workflow.ContainerType,
                workflow.NumberOfContainers,
                workflow.GrossWeight,
                workflow.Volume,
                workflow.DangerousGoods,
                workflow.TemperatureControlled,
                Services = ParseServices(workflow.SelectedServicesJson)
            },
            documents
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Buyer) ||
            string.IsNullOrWhiteSpace(request.Origin) ||
            string.IsNullOrWhiteSpace(request.Destination) ||
            string.IsNullOrWhiteSpace(request.Product))
        {
            return BadRequest(new
            {
                message = "Buyer, origin, destination and product are required."
            });
        }

        if (request.NumberOfContainers < 1)
            request.NumberOfContainers = 1;

        try
        {
            var now = DateTime.UtcNow;
            var tradeId = NewId("TRD");
            var shipmentId = NewId("SHP");
            var cargoId = NewId("CRG");
            var bookingReference = NewId("BK");

            var originLocation = await FindLocationAsync(request.Origin);
            var destinationLocation = await FindLocationAsync(request.Destination);

            var trade = new TradeOrder
            {
                TradeId = tradeId,
                TradeType = "Export",
                OriginCountry = originLocation?.Country,
                DestinationCountry = destinationLocation?.Country,
                Buyer = request.Buyer.Trim(),
                Seller = request.Exporter.Trim(),
                Commodity = request.Product.Trim(),
                TradeValue = ParseDecimal(request.ShipmentValue),
                Currency = request.Currency.Trim().ToUpperInvariant(),
                Incoterm = request.Incoterm.Trim().ToUpperInvariant(),
                PlannedShipDate = DateOnly.FromDateTime(now),
                RequiredDeliveryDate = request.RequiredDeliveryDate,
                Status = "Shipment Created"
            };

            var shipment = new Shipment
            {
                ShipmentId = shipmentId,
                TradeId = tradeId,
                Shipper = request.Exporter.Trim(),
                Consignee = request.Buyer.Trim(),
                Origin = request.Origin.Trim(),
                Destination = request.Destination.Trim(),
                PortOfLoading = originLocation?.Unlocode,
                PortOfDischarge = destinationLocation?.Unlocode,
                BookingReference = bookingReference,
                PlannedEta = request.RequiredDeliveryDate.HasValue
                    ? request.RequiredDeliveryDate.Value.ToDateTime(TimeOnly.MinValue)
                    : null,
                Status = "PLANNED"
            };

            var quantityInfo = ParseQuantity(request.Quantity);
            var cargo = new Cargo
            {
                CargoId = cargoId,
                TradeId = tradeId,
                Commodity = request.Product.Trim(),
                HsCode = string.IsNullOrWhiteSpace(request.HsCode) ? null : request.HsCode.Trim(),
                Quantity = quantityInfo.value,
                QuantityUnit = quantityInfo.unit,
                GrossWeight = ParseDecimal(request.GrossWeight),
                Volume = ParseDecimal(request.Volume),
                DangerousGoods = request.DangerousGoods.Trim(),
                ContainerRequirement = request.ContainerType.Trim(),
                Description = $"Shipment Mode: {request.ShipmentMode.Trim()}; Temperature Controlled: {(request.TemperatureControlled ? "Yes" : "No")}" 
            };

            var workflow = new ShipmentWorkflowDetail
            {
                ShipmentId = shipmentId,
                ShipmentMode = request.ShipmentMode.Trim(),
                ContainerType = request.ContainerType.Trim(),
                NumberOfContainers = request.NumberOfContainers,
                GrossWeight = ParseDecimal(request.GrossWeight),
                Volume = ParseDecimal(request.Volume),
                DangerousGoods = request.DangerousGoods.Trim(),
                TemperatureControlled = request.TemperatureControlled,
                SelectedServicesJson = JsonSerializer.Serialize(request.Services ?? new List<string>()),
                CreatedAt = now
            };

            _db.TradeOrders.Add(trade);
            _db.Shipments.Add(shipment);
            _db.Cargos.Add(cargo);
            _db.ShipmentWorkflowDetails.Add(workflow);

            var documentRequests = request.Documents?.Where(d => !string.IsNullOrWhiteSpace(d.DocumentType)).ToList()
                ?? new List<ShipmentDocumentRequest>();

            if (documentRequests.Count == 0)
            {
                documentRequests = DefaultDocuments(request.Services);
            }

            foreach (var document in documentRequests)
            {
                _db.Documents.Add(new Document
                {
                    DocumentId = NewId("DOC"),
                    TradeId = tradeId,
                    ShipmentId = shipmentId,
                    DocumentType = document.DocumentType.Trim(),
                    Status = string.IsNullOrWhiteSpace(document.Status) ? "Required" : document.Status.Trim(),
                    Version = 1
                });
            }

            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetShipment),
                new { shipmentId },
                new
                {
                    shipmentId,
                    tradeId,
                    bookingReference,
                    status = shipment.Status,
                    message = $"Shipment {shipmentId} created successfully."
                });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                message = "The shipment could not be saved to the database.",
                detail = ex.InnerException?.Message ?? ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred while creating the shipment.",
                detail = ex.Message
            });
        }
    }

    private async Task<Location?> FindLocationAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var trimmed = input.Trim();

        return await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Name == trimmed ||
                x.Unlocode == trimmed ||
                x.LocationId == trimmed ||
                x.Name.Contains(trimmed));
    }

    private static decimal? ParseDecimal(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        // The reference UI accepts Indian-style thousands separators such as
        // 18,60,000. Remove separators before extracting the numeric value.
        var cleaned = text.Replace(",", string.Empty).Trim();
        var match = Regex.Match(cleaned, @"[-+]?\d+(?:\.\d+)?");
        if (!match.Success)
            return null;

        return decimal.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? value
            : null;
    }

    private static (decimal? value, string? unit) ParseQuantity(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return (null, null);

        var cleaned = text.Replace(",", string.Empty).Trim();
        var match = Regex.Match(cleaned, @"^(?<value>[-+]?\d+(?:\.\d+)?)\s*(?<unit>.*)$");
        if (!match.Success)
            return (ParseDecimal(text), null);

        var normalized = match.Groups["value"].Value;
        if (!decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return (null, null);

        var unit = match.Groups["unit"].Value.Trim();
        return (value, string.IsNullOrWhiteSpace(unit) ? null : unit);
    }

    private static string NewId(string prefix)
        => $"{prefix}-{Guid.NewGuid():N}"[..Math.Min(19, prefix.Length + 1 + 13)];

    private static List<ShipmentDocumentRequest> DefaultDocuments(List<string>? services)
    {
        var docs = new List<ShipmentDocumentRequest>
        {
            new() { DocumentType = "Commercial Invoice", Status = "Required" },
            new() { DocumentType = "Packing List", Status = "Required" },
            new() { DocumentType = "Certificate of Origin", Status = "Required" },
            new() { DocumentType = "Shipping Bill", Status = "Pending" },
            new() { DocumentType = "Bill of Lading", Status = "Generated after booking" }
        };

        if (services?.Contains("insurance", StringComparer.OrdinalIgnoreCase) == true)
            docs.Add(new ShipmentDocumentRequest { DocumentType = "Insurance Certificate", Status = "Required if insurance selected" });

        return docs;
    }

    private static List<string> ParseServices(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
