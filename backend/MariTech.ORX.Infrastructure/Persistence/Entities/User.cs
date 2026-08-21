using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? OrganisationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Organisation? Organisation { get; set; }
}
