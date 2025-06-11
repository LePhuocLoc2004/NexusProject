using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class FinanceServiceImpl : FinanceService
{
    private DatabaseContext db;

    public FinanceServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }
    public Finance CreateFinance(int customerId, int connectionId, double totalAmount, string paymentMethod, double paymentAmount)
    {
        double amountPaid = 0;
        double remainingAmount = 0;

        if (paymentMethod == "PayPal")
        {
            amountPaid = paymentAmount;
            remainingAmount = totalAmount - amountPaid;
        }
        else if (paymentMethod == "PayLater")
        {
            amountPaid = 0;
            remainingAmount = totalAmount;
        }

        var finance = new Finance
        {
            CustomerId = customerId,
            ConnectionId = connectionId,
            TotalAmount = totalAmount,
            AmountPaid = amountPaid,
            RemainingAmount = remainingAmount
        };

        db.Finances.Add(finance);
        db.SaveChanges();

        return finance;
    }
    //
    public List<Finance> GetAllFinances()
    {
        return db.Finances.ToList();
    }

    public double GetTotalAmountSum()
    {
        return db.Finances.Sum(f => (f.TotalAmount ?? 0));
    }

    public int GetTotalCustomers()
    {
        return db.Finances.Select(f => f.CustomerId).Distinct().Count();
    }

    public double GetTotalAmountPaid()
    {
        return db.Finances.Sum(f => f.AmountPaid);
    }

    public double GetTotalRemainingAmount()
    {
        return db.Finances.Sum(f => (f.TotalAmount - f.AmountPaid ?? 0));
    }
}
