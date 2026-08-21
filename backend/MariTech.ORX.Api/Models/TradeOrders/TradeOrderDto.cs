namespace MariTech.ORX.Api.Models.TradeOrders;

public class TradeOrderDto
{
    public string TradeId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public string? TradeType { get; set; }

    public string? OriginCountry { get; set; }

    public string? DestinationCountry { get; set; }

    public string? Buyer { get; set; }

    public string? Seller { get; set; }

    public string? Commodity { get; set; }

    public decimal? TradeValue { get; set; }

    public string? Currency { get; set; }

    public string? Incoterm { get; set; }

    public string? PaymentTerms { get; set; }

    public DateOnly? PlannedShipDate { get; set; }

    public DateOnly? RequiredDeliveryDate { get; set; }

    public string? Status { get; set; }
}