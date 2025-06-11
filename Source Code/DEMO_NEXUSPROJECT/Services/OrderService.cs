using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface OrderService
{
    public List<Order> findAll(int customerId);
    public List<Order> latestOrder(int n);
}
