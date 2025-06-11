using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface InvoiceEmployeeService
{
    public List<Invoice> findAll();
    public bool update(Invoice invoice);
    public bool delete(int invoiceId);
    public Invoice findById(int customerId);
    public Invoice findByInvoiceId(int invoiceId);
    public bool create(Invoice invoice);
}
