using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/health")]
public class HealthController : ControllerBase
{
    private readonly ORXDbContext _db;

    public HealthController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            bool connected = await _db.Database.CanConnectAsync();

            return Ok(new
            {
                status = "ok",
                database = connected ? "connected" : "not_connected",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "error",
                database = "not_connected",
                message = ex.Message
            });
        }
    }
}