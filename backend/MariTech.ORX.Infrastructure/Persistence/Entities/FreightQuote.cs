using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class FreightQuote
{
    public string QuoteId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public string? Origin { get; set; }

    public string? Destination { get; set; }

    public string? Carrier { get; set; }

    public string? ContainerType { get; set; }

    public int? EquipmentQuantity { get; set; }

    public decimal? BaseFreight { get; set; }

    public decimal? Baf { get; set; }

    public decimal? LocalCharges { get; set; }

    public decimal? CongestionSurcharge { get; set; }

    public decimal? OtherSurcharges { get; set; }

    public decimal? TotalCost { get; set; }

    public string? Currency { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidUntil { get; set; }

    public string? Source { get; set; }

    public virtual Organisation? Customer { get; set; }
}
