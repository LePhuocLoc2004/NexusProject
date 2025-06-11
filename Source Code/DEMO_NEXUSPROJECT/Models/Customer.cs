using System;
using System.Collections.Generic;

namespace DEMO_NEXUSPROJECT.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; }

    public string CityCode { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string? Idcard { get; set; }

    public string Password { get; set; }

    public bool Status { get; set; }

    public string SecurityCode { get; set; }

    public string? Photo { get; set; }

    public virtual ICollection<ConnectionRequest> ConnectionRequests { get; set; } = new List<ConnectionRequest>();

    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();

    public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; } = new List<CustomerFeedback>();

    public virtual ICollection<CustomerServicePackage> CustomerServicePackages { get; set; } = new List<CustomerServicePackage>();

    public virtual ICollection<Finance> Finances { get; set; } = new List<Finance>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
}
