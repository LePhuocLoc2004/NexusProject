using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public bool Status { get; set; }

    public string SecurityCode { get; set; }

    public string Photo { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
