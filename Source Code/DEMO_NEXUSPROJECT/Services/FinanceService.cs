using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface FinanceService
{
    public Finance CreateFinance(int customerId, int connectionId, double totalAmount, string paymentMethod, double paymentAmount);
    //
    public List<Finance> GetAllFinances();

    public double GetTotalAmountSum();

    public int GetTotalCustomers();

    public double GetTotalAmountPaid();

    public double GetTotalRemainingAmount();
}
