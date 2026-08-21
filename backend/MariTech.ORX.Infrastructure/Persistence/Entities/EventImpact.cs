using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class EventImpact
{
    public string EventId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string ShipmentId { get; set; } = null!;

    public string? ImpactType { get; set; }

    public decimal? ImpactScore { get; set; }

    public decimal? Probability { get; set; }

    public decimal? EstimatedDelay { get; set; }

    public decimal? EstimatedCost { get; set; }

    public string? RecommendedAction { get; set; }

    public virtual NetworkEvent Event { get; set; } = null!;

    public virtual TradeOrder Trade { get; set; } = null!;
}
