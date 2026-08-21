using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Location
{
    public string LocationId { get; set; } = null!;

    public string? Unlocode { get; set; }

    public string Name { get; set; } = null!;

    public string? Country { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? LocationType { get; set; }

    public virtual ICollection<Port> Ports { get; set; } = new List<Port>();
}
