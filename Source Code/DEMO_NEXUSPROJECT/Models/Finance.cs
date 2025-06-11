using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Finance
{
    public int FinanceId { get; set; }

    public int CustomerId { get; set; }

    public int ConnectionId { get; set; }

    public double? TotalAmount { get; set; }

    public double AmountPaid { get; set; }

    public double? RemainingAmount { get; set; }

    public virtual Connection Connection { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
