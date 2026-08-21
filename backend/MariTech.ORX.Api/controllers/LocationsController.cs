using MariTech.ORX.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MariTech.ORX.Api.Controllers;

[ApiController]
[Route("api/v1/locations")]
public class LocationsController : ControllerBase
{
    private readonly ORXDbContext _db;

    public LocationsController(ORXDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _db.Locations
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                x.LocationId,
                x.Unlocode,
                x.Name,
                x.Country,
                x.Latitude,
                x.Longitude,
                x.LocationType
            })
            .ToListAsync();

        return Ok(locations);
    }
}
