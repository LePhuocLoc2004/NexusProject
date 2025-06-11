using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface OrderEmployeeService
{
    public List<Order> findAll();
    public bool update(Order order);
    public bool delete(int orderId);
    public Order findById(int orderId);
    public bool create(Order order);
}
