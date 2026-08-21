using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Port
{
    public string PortId { get; set; } = null!;

    public string LocationId { get; set; } = null!;

    public string? PortAuthority { get; set; }

    public string? Country { get; set; }

    public virtual Location Location { get; set; } = null!;
}
