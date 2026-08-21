using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class TradeOrder
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

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();

    public virtual Organisation? Customer { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<EventImpact> EventImpacts { get; set; } = new List<EventImpact>();

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual ICollection<RiskAssessment> RiskAssessments { get; set; } = new List<RiskAssessment>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
