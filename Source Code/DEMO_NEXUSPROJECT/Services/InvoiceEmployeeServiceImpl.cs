using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class InvoiceEmployeeServiceImpl : InvoiceEmployeeService
{
    private DatabaseContext db;
    public InvoiceEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;

    }

    public bool create(Invoice invoice)
    {
        try
        {
            db.Invoices.Add(invoice);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool delete(int invoiceId)
    {
        try
        {
            db.Invoices.Remove(db.Invoices.Find(invoiceId));
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<Invoice> findAll()
    {
        return db.Invoices.ToList();
    }

    public Invoice findById(int customerId)
    {
        return db.Invoices.Find(customerId);
    }

    public Invoice findByInvoiceId(int invoiceId)
    {
        return db.Invoices.Find(invoiceId);
    }

    public bool update(Invoice invoice)
    {
        try
        {
            db.Entry(invoice).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
}
