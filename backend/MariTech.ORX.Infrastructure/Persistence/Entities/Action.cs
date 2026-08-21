using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Action
{
    public string ActionId { get; set; } = null!;

    public string RecommendationId { get; set; } = null!;

    public string? Actor { get; set; }

    public string? ActionType { get; set; }

    public string? Target { get; set; }

    public string? Status { get; set; }

    public DateTime? RequestedAt { get; set; }

    public virtual User? ActorNavigation { get; set; }

    public virtual Recommendation Recommendation { get; set; } = null!;
}
