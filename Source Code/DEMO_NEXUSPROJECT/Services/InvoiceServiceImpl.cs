using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DEMO_NEXUSPROJECT.Services;

public class InvoiceServiceImpl : InvoiceService
{
    private DatabaseContext db;

    public InvoiceServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }
    public Invoice creteInvoice(int customerId, double amount,string status)
    {
        var InvoiceCode = GenerateOrderCode.GenerateOrderCode2();
        var invoice = new Invoice
        {
            InvoiceNumber = InvoiceCode,
            CustomerId = customerId,
            Amount = amount,
            IssueDate = DateTime.Now,
            Status = status
        };

        db.Invoices.Add(invoice);
        db.SaveChanges();

        return invoice;
    }

    public Invoice findByID(int invoiceId)
    {
        var invoice = db.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
        if (invoice == null)
        {
            throw new Exception("Invoice not found");
        }
        return invoice;
    }


    public Invoice GetInvoice(int invoiceId)
    {
        return db.Invoices
                       .Include("Customer")
                       .Include("Payments")
                       .FirstOrDefault(i => i.InvoiceId == invoiceId);
    }

    public List<Invoice> GetAllInvoices()
    {
        return db.Invoices
                       .Include("Customer")
                       .Include("Payments")
                       .ToList();
    }

    //public Invoice CreateInvoice(int customerId, int servicePackageId, int deviceId)
    //{
    //    // Lấy thông tin gói dịch vụ
    //    var servicePackage = db.ServicePackages.Find(servicePackageId);
    //    if (servicePackage == null)
    //    {
    //        throw new Exception("Service package not found");
    //    }

    //    // Tính toán chi phí và thuế dịch vụ
    //    var servicePrice = servicePackage.Price ?? 0m;
    //    var tax = servicePrice * 0.1224m;
    //    var totalAmount = servicePrice + tax;

    //    // Tạo hóa đơn mới
    //    var invoice = new Invoice
    //    {
    //        InvoiceNumber = GenerateInvoiceNumber(),
    //        CustomerId = customerId,
    //        Amount = totalAmount, // Giả sử bạn có một phương thức để tính toán tổng số tiền
    //        Status = "Unpaid",
    //        IssueDate = DateTime.Now,
    //        Payments = new List<Payment>()
    //    };

    //    db.Invoices.Add(invoice);
    //    db.SaveChanges();

    //    return invoice;
    //}

    //private string GenerateInvoiceNumber()
    //{
    //    // Tạo số hóa đơn duy nhất
    //    return "INV" + DateTime.Now.Ticks;
    //}

    public List<Invoice> GetUnpaidInvoices(int customerId)
    {
        return db.Invoices.Where(i => i.CustomerId == customerId && i.PaymentDate == null).ToList();
    }

    public List<Invoice> findByKeyword(string keyword)
    {
        return db.Invoices.Where(p => p.Customer.FullName.Contains(keyword)).ToList();
    }

    public bool DeleteInvoice(int invoiceId)
    {
        var invoice = db.Invoices
                    .Include(i => i.Payments) // Include payments related to the invoice
                    .SingleOrDefault(i => i.InvoiceId == invoiceId);

        if (invoice == null)
        {
            return false; // Or handle case where invoice is not found
        }

        // Optionally, you can handle or process payments associated with the invoice here
        // For example, you might want to delete associated payments or mark them as processed.

        // Delete associated payments if needed
        foreach (var payment in invoice.Payments.ToList())
        {
            db.Payments.Remove(payment);
        }

        // Now remove the invoice itself
        db.Invoices.Remove(invoice);
        db.SaveChanges();
        return true;
    }

    public List<Invoice> GetInvoicesByMonth(int month, int year)
    {
        return db.Invoices
            .Where(i => i.IssueDate.Month == month && i.IssueDate.Year == year)
            .ToList();
    }

    public List<Invoice> GetInvoicesByQuarter(int quarter, int year)
    {
        return db.Invoices
            .Where(i => (i.IssueDate.Month - 1) / 3 + 1 == quarter && i.IssueDate.Year == year)
            .ToList();
    }

    public List<Invoice> GetInvoicesByYear(int year)
    {
        return db.Invoices
            .Where(i => i.IssueDate.Year == year)
            .ToList();
    }

    public double CalculateTotalAmount()
    {
        return db.Invoices.Sum(invoice => invoice.Amount);
    }

    public List<Invoice> GetInvoicePaged(int page, int pageSize)
    {
        return db.Invoices
                       .OrderBy(i => i.InvoiceId)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();
    }

    public int GetTotalInvoices()
    {
        return db.Invoices.Count();
    }

    public IEnumerable<Invoice> GetInvoicesByYear(int year, int page, int pageSize)
    {
        return db.Invoices
            .Where(i => i.IssueDate.Year == year)
            .OrderBy(i => i.InvoiceNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetInvoicesByYearCount(int year)
    {
        return db.Invoices.Count(i => i.IssueDate.Year == year);
    }

    public IEnumerable<Invoice> GetInvoicesByMonth(int month, int year, int page, int pageSize)
    {
        return db.Invoices
            .Where(i => i.IssueDate.Month == month && i.IssueDate.Year == year)
            .OrderBy(i => i.InvoiceNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetTotalInvoicesByMonth(int month, int year)
    {
        return db.Invoices.Count(i => i.IssueDate.Month == month && i.IssueDate.Year == year);
    }

    public IEnumerable<Invoice> GetInvoicesByQuarter(int quarter, int year, int page, int pageSize)
    {
        var startMonth = (quarter - 1) * 3 + 1;
        var endMonth = startMonth + 2;
        return db.Invoices
            .Where(i => i.IssueDate.Year == year && i.IssueDate.Month >= startMonth && i.IssueDate.Month <= endMonth)
            .OrderBy(i => i.InvoiceNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public int GetTotalInvoicesByQuarter(int quarter, int year)
    {
        var startMonth = (quarter - 1) * 3 + 1;
        var endMonth = startMonth + 2;
        return db.Invoices.Count(i => i.IssueDate.Year == year && i.IssueDate.Month >= startMonth && i.IssueDate.Month <= endMonth);
    }
}
