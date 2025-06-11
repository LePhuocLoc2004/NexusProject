using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class RetailStore
{
    public int StoreId { get; set; }

    public string? StoreName { get; set; }

    public string? Location { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
