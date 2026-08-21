using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class VoyageSchedule
{
    public string VoyageId { get; set; } = null!;

    public string ServiceId { get; set; } = null!;

    public string VesselId { get; set; } = null!;

    public string? VoyageNumber { get; set; }

    public string? PortSequence { get; set; }

    public DateTime? PlannedArrival { get; set; }

    public DateTime? PlannedDeparture { get; set; }

    public string? Status { get; set; }

    public virtual CarrierService Service { get; set; } = null!;

    public virtual Vessel Vessel { get; set; } = null!;
}
