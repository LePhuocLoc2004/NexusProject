using DEMO_NEXUSPROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_NEXUSPROJECT.Services;

public class PaymentServiceImpl : PaymentService
{
    private DatabaseContext db;
    public PaymentServiceImpl(DatabaseContext db)
    {
        this.db = db;
    }

    public Payment createPayment(int invoiceId, double amount, string paymentMethod)
    {
        
        var payment = new Payment
        {
            InvoiceId = invoiceId,
            PaymentDate = DateTime.Now,
            Amount = amount,
            PaymentMethod = paymentMethod
        };
        db.Payments.AddRange(payment);
        db.SaveChanges();

        return payment;
    }
    public void RecordPayment(int invoiceId, double amountPaid, string paymentMethod)
    {
        var invoice = db.Invoices.Find(invoiceId);
        if (invoice == null)
        {
            throw new Exception("Invoice not found");
        }

        if (invoice.Status == "Paid")
        {
            throw new Exception("Invoice is already paid");
        }

        // Update invoice status and amount paid
        invoice.Status = "Paid";
        invoice.PaymentDate = DateTime.Now;

        // Create payment record
        var payment = new Payment
        {

            InvoiceId = invoiceId,
            Amount = amountPaid,
            PaymentMethod = paymentMethod,
            PaymentDate = DateTime.Now
        };

        db.Payments.Add(payment);

        // Update invoice status to "Paid" if full payment is received
        if (invoice.Amount >= invoice.Amount)
        {
            invoice.Status = "Paid";
        }
        db.SaveChanges();
    }




    public List<Payment> GetPaymentsByInvoice(int invoiceId)
    {
        return db.Payments
            .Where(p => p.InvoiceId == invoiceId)
            .ToList();
    }

    public List<Payment> GetPaymentsByMonth(int month, int year)
    {
        return db.Payments
            .Where(p => p.PaymentDate.Month == month && p.PaymentDate.Year == year)
            .ToList();
    }

    public List<Payment> GetPaymentsByQuarter(int quarter, int year)
    {
        return db.Payments
            .Where(p => (p.PaymentDate.Month - 1) / 3 + 1 == quarter && p.PaymentDate.Year == year)
            .ToList();
    }

    public List<Payment> GetPaymentsByYear(int year)
    {
        return db.Payments
            .Where(p => p.PaymentDate.Year == year)
            .ToList();
    }
}
