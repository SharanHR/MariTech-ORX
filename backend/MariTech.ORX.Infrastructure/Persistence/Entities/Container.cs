using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Container
{
    public string ContainerId { get; set; } = null!;

    public string ShipmentId { get; set; } = null!;

    public string? ContainerNumber { get; set; }

    public string? ContainerType { get; set; }

    public string? IsoCode { get; set; }

    public string? Carrier { get; set; }

    public string? CurrentLocation { get; set; }

    public string? Status { get; set; }

    public string? Availability { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Volume { get; set; }

    public string? SealNumber { get; set; }
}
