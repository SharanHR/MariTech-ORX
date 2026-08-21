namespace MariTech.ORX.Api.Models.RiskAssessments;

public class RiskAssessmentDto
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
}