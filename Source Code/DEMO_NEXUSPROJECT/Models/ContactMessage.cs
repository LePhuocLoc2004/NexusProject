using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class ContactMessage
{
    public int ContactId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;
}
