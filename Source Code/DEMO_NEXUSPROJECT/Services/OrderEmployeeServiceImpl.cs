using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class OrderEmployeeServiceImpl : OrderEmployeeService
{
    private DatabaseContext db;
    public OrderEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;

    }

    public bool create(Order order)
    {
        try
        {
            db.Orders.Add(order);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool delete(int orderId)
    {
        try
        {
            db.Invoices.Remove(db.Invoices.Find(orderId));
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<Order> findAll()
    {
        return db.Orders.ToList();
    }

    public Order findById(int orderId)
    {
        return db.Orders.Find(orderId);
    }


    public bool update(Order order)
    {
        try
        {
            db.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
}
