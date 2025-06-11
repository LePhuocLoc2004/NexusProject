using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class ConnectionRequest
{
    public int RequestId { get; set; }

    public int CustomerId { get; set; }

    public string? ConnectionType { get; set; }

    public DateTime RequestDate { get; set; }

    public string? Status { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
