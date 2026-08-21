using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class RiskAssessment
{
    public string RiskId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string ShipmentId { get; set; } = null!;

    public string? RiskType { get; set; }

    public decimal? Score { get; set; }

    public decimal? Probability { get; set; }

    public string? Severity { get; set; }

    public string? Reason { get; set; }

    public string? Evidence { get; set; }

    public DateTime? CalculatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public virtual TradeOrder Trade { get; set; } = null!;
}
