using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Recommendation
{
    public string RecommendationId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string? TriggerEvent { get; set; }

    public string? RecommendationType { get; set; }

    public string? Action { get; set; }

    public string? Reason { get; set; }

    public string? ExpectedBenefit { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? RiskReduction { get; set; }

    public decimal? Confidence { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual TradeOrder Trade { get; set; } = null!;
}
