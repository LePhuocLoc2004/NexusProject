using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class ConnectionPackage
{
    public int ConnectionId { get; set; }

    public int ServicePackageId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Connection Connection { get; set; } = null!;

    public virtual ServicePackage ServicePackage { get; set; } = null!;
}
