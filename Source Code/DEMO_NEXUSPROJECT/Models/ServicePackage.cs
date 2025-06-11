using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class ServicePackage
{
    public int ServicePackageId { get; set; }

    public string ServicePackageName { get; set; } = null!;

    public string ConnectionType { get; set; }

    public double Price { get; set; }

    public int? Duration { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<ConnectionPackage> ConnectionPackages { get; set; } = new List<ConnectionPackage>();

    public virtual ICollection<CustomerServicePackage> CustomerServicePackages { get; set; } = new List<CustomerServicePackage>();
}
