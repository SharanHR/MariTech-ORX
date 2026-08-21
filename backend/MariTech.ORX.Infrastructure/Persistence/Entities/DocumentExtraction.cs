using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class DocumentExtraction
{
    public string DocumentId { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? Value { get; set; }

    public decimal? Confidence { get; set; }

    public string? ExtractionModel { get; set; }

    public virtual Document Document { get; set; } = null!;
}
