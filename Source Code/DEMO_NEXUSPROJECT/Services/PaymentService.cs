using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface PaymentService
{
    public Payment createPayment(int invoiceId, double amount, string paymentMethod);
    //
    public void RecordPayment(int invoiceId, double amountPaid, string paymentMethod);

    public List<Payment> GetPaymentsByInvoice(int invoiceId);

    public List<Payment> GetPaymentsByMonth(int month, int year);

    public List<Payment> GetPaymentsByQuarter(int quarter, int year);

    public List<Payment> GetPaymentsByYear(int year);
}
