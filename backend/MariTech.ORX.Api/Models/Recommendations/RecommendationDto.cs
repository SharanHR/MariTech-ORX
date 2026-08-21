namespace MariTech.ORX.Api.Models.Recommendations;

public class RecommendationDto
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
}