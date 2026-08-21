namespace MariTech.ORX.Api.Models.Actions;

public class ActionDto
{
    public string ActionId { get; set; } = null!;

    public string RecommendationId { get; set; } = null!;

    public string? Actor { get; set; }

    public string? ActionType { get; set; }

    public string? Target { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestedAt { get; set; }
}