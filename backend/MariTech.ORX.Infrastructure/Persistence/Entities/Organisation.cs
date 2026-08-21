using System;
using System.Collections.Generic;

namespace MariTech.ORX.Infrastructure.Persistence.Entities;

public partial class Organisation
{
    public string OrganisationId { get; set; } = null!;

    public string LegalName { get; set; } = null!;

    public string? TradeName { get; set; }

    public string? OrganisationType { get; set; }

    public string? Country { get; set; }

    public string? Industry { get; set; }

    public decimal? AnnualTradeVolume { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<FreightQuote> FreightQuotes { get; set; } = new List<FreightQuote>();

    public virtual ICollection<TradeOrder> TradeOrders { get; set; } = new List<TradeOrder>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
