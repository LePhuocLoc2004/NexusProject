using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Connection
{
    public int ConnectionId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public string ConnectionType { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime ActivationDate { get; set; }

    public DateTime TerminationDate { get; set; }

    public virtual ICollection<ConnectionPackage> ConnectionPackages { get; set; } = new List<ConnectionPackage>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerServicePackage> CustomerServicePackages { get; set; } = new List<CustomerServicePackage>();

    public virtual ICollection<Finance> Finances { get; set; } = new List<Finance>();
}
