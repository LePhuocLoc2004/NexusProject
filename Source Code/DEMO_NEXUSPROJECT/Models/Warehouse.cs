using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public int DeviceId { get; set; }

    public int StoreId { get; set; }

    public int? Quantity { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual RetailStore Store { get; set; } = null!;
}
