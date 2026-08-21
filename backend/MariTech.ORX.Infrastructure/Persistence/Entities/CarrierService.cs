using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class CarrierService
{
    public string ServiceId { get; set; } = null!;

    public string? Carrier { get; set; }

    public string? ServiceName { get; set; }

    public string? TradeLane { get; set; }

    public string? Frequency { get; set; }

    public virtual ICollection<VoyageSchedule> VoyageSchedules { get; set; } = new List<VoyageSchedule>();
}
