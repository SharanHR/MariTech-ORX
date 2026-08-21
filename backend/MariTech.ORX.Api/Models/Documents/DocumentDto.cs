namespace MariTech.ORX.Api.Models.Documents;

public class DocumentDto
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
}