using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Device
{
    public int DeviceId { get; set; }

    public string DeviceName { get; set; } = null!;

    public string? DeviceType { get; set; }

    public int? StockQuantity { get; set; }

    public int? SupplierId { get; set; }

    public virtual ICollection<DeviceOrder> DeviceOrders { get; set; } = new List<DeviceOrder>();

    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
