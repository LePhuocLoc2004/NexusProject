using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class CustomerFeedback
{
    public int FeedbackId { get; set; }

    public int CustomerId { get; set; }

    public DateTime FeedbackDate { get; set; }

    public string? Feedback { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
