using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class NetworkEvent
{
    public string EventId { get; set; } = null!;

    public string? EventType { get; set; }

    public string? Severity { get; set; }

    public string? Location { get; set; }

    public string? Carrier { get; set; }

    public string? Vessel { get; set; }

    public string? TradeLane { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }

    public string? Source { get; set; }

    public decimal? Confidence { get; set; }

    public virtual ICollection<EventImpact> EventImpacts { get; set; } = new List<EventImpact>();

    public virtual Vessel? VesselNavigation { get; set; }
}
