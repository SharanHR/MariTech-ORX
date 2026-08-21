using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ORXDbContext _db;

    public DashboardController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var totalShipments = await _db.Shipments.CountAsync();

        var inTransit = await _db.Shipments
            .CountAsync(x => x.Status == "In Transit");

        var delivered = await _db.Shipments
            .CountAsync(x => x.Status == "Delivered");

        var pending = await _db.Shipments
            .CountAsync(x =>
                x.Status != "Delivered" &&
                x.Status != "In Transit");

        var highRisk = await _db.Shipments
            .CountAsync(x => x.RiskScore >= 70);

        return Ok(new
        {
            totalShipments,
            inTransit,
            delivered,
            pending,
            highRisk
        });
    }
}