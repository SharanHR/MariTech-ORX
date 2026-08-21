namespace MariTech.ORX.Api.Models.Shipments;

public class CreateShipmentRequest
{
    // Trade details
    public string Exporter { get; set; } = string.Empty;
    public string Buyer { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public string? HsCode { get; set; }
    public string? Quantity { get; set; }
    public string? ShipmentValue { get; set; }
    public string Currency { get; set; } = "INR";
    public string Incoterm { get; set; } = "FOB";
    public DateOnly? RequiredDeliveryDate { get; set; }

    // Cargo details
    public string ShipmentMode { get; set; } = "Ocean FCL";
    public string ContainerType { get; set; } = "20' Standard";
    public int NumberOfContainers { get; set; } = 1;
    public string? GrossWeight { get; set; }
    public string? Volume { get; set; }
    public string DangerousGoods { get; set; } = "No";
    public bool TemperatureControlled { get; set; }

    // Requested services
    public List<string> Services { get; set; } = new();

    // Required documents and their current workflow state.
    public List<ShipmentDocumentRequest> Documents { get; set; } = new();
}

public class ShipmentDocumentRequest
{
    public string DocumentType { get; set; } = string.Empty;
    public string Status { get; set; } = "Required";
}
