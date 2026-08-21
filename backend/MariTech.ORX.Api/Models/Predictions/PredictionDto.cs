namespace MariTech.ORX.Api.Models.Predictions;

public class PredictionDto
{
    public string PredictionId { get; set; } = null!;

    public string? EntityType { get; set; }

    public string? EntityId { get; set; }

    public string? PredictionType { get; set; }

    public decimal? PredictedValue { get; set; }

    public decimal? Probability { get; set; }

    public string? PredictionHorizon { get; set; }

    public string? ModelVersion { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }
}