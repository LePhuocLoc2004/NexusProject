using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class OrderServiceImpl : OrderService
{
    private DatabaseContext db;

    public OrderServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }
    public List<Order> findAll(int customerId)
    {
       return db.Orders.Where(o =>  o.CustomerId == customerId).ToList();
    }

    public List<Order> latestOrder(int n)
    {
        return db.Orders.OrderByDescending(p => p.OrderId).Take(n).ToList();
    }
}
