using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface InvoiceService
{
    public Invoice creteInvoice(int customerId, double amount,string status);
    public Invoice findByID(int invoiceId);
    //
    public Invoice GetInvoice(int invoiceId);

    public List<Invoice> GetAllInvoices();

    public double CalculateTotalAmount();
    public List<Invoice> findByKeyword(string keyword);

    public bool DeleteInvoice(int id);

    public List<Invoice> GetInvoicesByMonth(int month, int year);

    public List<Invoice> GetInvoicesByQuarter(int quarter, int year);

    public List<Invoice> GetInvoicesByYear(int year);

    public List<Invoice> GetInvoicePaged(int page, int pageSize);

    public IEnumerable<Invoice> GetInvoicesByYear(int year, int page, int pageSize);

    public IEnumerable<Invoice> GetInvoicesByMonth(int month, int year, int page, int pageSize);

    public IEnumerable<Invoice> GetInvoicesByQuarter(int quarter, int year, int page, int pageSize);

    public int GetTotalInvoices();

    public int GetInvoicesByYearCount(int year);

    public int GetTotalInvoicesByMonth(int month, int year);

    public int GetTotalInvoicesByQuarter(int quarter, int year);




}
