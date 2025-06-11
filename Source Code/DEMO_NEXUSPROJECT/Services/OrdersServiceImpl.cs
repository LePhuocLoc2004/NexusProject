using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_NEXUSPROJECT.Services;

public class OrdersServiceImpl : OrdersService
{
    private DatabaseContext db;

    public OrdersServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }
    public Order createOrder(int customerId, string connectionType)
    {
       
        var orderCode = GenerateOrderCode.GenerateOrderCode1(connectionType);
        
        var order = new Order
        {
            OrderCode = orderCode,
            CustomerId = customerId,
            ConnectionType = connectionType,
            OrderDate = DateTime.Now,
            Status = "Completed"
        };

        db.Orders.Add(order);
        db.SaveChanges();

        return order;
    }
}
