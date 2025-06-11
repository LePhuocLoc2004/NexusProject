using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string? Description { get; set; }

    public double? DiscountRate { get; set; }

    public string? Conditions { get; set; }
}
