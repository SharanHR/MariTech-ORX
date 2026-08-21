namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class ShipmentWorkflowDetail
{
    public string ShipmentId { get; set; } = null!;
    public string? ShipmentMode { get; set; }
    public string? ContainerType { get; set; }
    public int? NumberOfContainers { get; set; }
    public decimal? GrossWeight { get; set; }
    public decimal? Volume { get; set; }
    public string? DangerousGoods { get; set; }
    public bool? TemperatureControlled { get; set; }
    public string? SelectedServicesJson { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
}
