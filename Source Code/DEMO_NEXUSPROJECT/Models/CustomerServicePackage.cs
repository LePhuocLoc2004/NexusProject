using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class CustomerServicePackage
{
    public int CustomerId { get; set; }

    public int ConnectionId { get; set; }

    public int ServicePackageId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public virtual Connection Connection { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ServicePackage ServicePackage { get; set; } = null!;
}
