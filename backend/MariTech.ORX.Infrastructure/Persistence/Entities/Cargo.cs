using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Cargo
{
    public string CargoId { get; set; } = null!;

    public string TradeId { get; set; } = null!;

    public string? Commodity { get; set; }

    public string? HsCode { get; set; }

    public string? Description { get; set; }

    public decimal? Quantity { get; set; }

    public string? QuantityUnit { get; set; }

    public decimal? GrossWeight { get; set; }

    public decimal? NetWeight { get; set; }

    public decimal? Volume { get; set; }

    public string? PackagingType { get; set; }

    public string? DangerousGoods { get; set; }

    public string? ContainerRequirement { get; set; }

    public virtual TradeOrder Trade { get; set; } = null!;
}
