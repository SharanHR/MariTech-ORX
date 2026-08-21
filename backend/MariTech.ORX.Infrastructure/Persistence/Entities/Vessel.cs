using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Vessel
{
    public string VesselId { get; set; } = null!;

    public string? ImoNumber { get; set; }

    public string? Mmsi { get; set; }

    public string? VesselName { get; set; }

    public string? Carrier { get; set; }

    public string? VesselType { get; set; }

    public int? CapacityTeu { get; set; }

    public string? Flag { get; set; }

    public virtual ICollection<NetworkEvent> NetworkEvents { get; set; } = new List<NetworkEvent>();

    public virtual ICollection<VoyageSchedule> VoyageSchedules { get; set; } = new List<VoyageSchedule>();
}
