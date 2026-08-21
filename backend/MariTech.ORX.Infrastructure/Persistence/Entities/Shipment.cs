using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Shipment
{
    public string ShipmentId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string? Shipper { get; set; }

    public string? Consignee { get; set; }

    public string? Forwarder { get; set; }

    public string? Carrier { get; set; }

    public string? BookingReference { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? PortOfLoading { get; set; }

    public string? PortOfDischarge { get; set; }

    public DateTime? PlannedEtd { get; set; }

    public DateTime? PlannedEta { get; set; }

    public DateTime? CurrentEtd { get; set; }

    public DateTime? CurrentEta { get; set; }

    public string? Status { get; set; }

    public decimal? RiskScore { get; set; }

    public virtual TradeOrder Trade { get; set; } = null!;
}
