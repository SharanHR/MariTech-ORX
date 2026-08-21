using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Document
{
    public string DocumentId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string ShipmentId { get; set; } = null!;

    public string? DocumentType { get; set; }

    public string? DocumentNumber { get; set; }

    public string? Issuer { get; set; }

    public DateOnly? IssueDate { get; set; }

    public string? Status { get; set; }

    public int? Version { get; set; }

    public virtual ICollection<DocumentExtraction> DocumentExtractions { get; set; } = new List<DocumentExtraction>();

    public virtual TradeOrder Trade { get; set; } = null!;
}
