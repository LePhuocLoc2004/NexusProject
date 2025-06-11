using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class TransactionLog
{
    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public string? TransactionType { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Details { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
