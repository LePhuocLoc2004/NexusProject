using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class DeviceOrder
{
    public int DeviceOrderId { get; set; }

    public int OrderId { get; set; }

    public int DeviceId { get; set; }

    public int? Quantity { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
