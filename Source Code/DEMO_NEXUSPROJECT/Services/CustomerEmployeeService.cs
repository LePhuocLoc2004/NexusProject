using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface CustomerEmployeeService
{
    public List<Customer> findAll();
    public bool update(Customer customer);
    public bool delete(int customerId);
    public Customer findById(int customerId);
    public bool create(Customer customer);
    public int CountCustomers();
}
