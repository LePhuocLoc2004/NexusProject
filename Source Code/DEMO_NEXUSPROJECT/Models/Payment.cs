using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int InvoiceId { get; set; }

    public DateTime PaymentDate { get; set; }

    public double Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
