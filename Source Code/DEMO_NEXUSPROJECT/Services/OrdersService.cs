using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface OrdersService
{
    public Order createOrder(int customerId, string connectionType);

}
