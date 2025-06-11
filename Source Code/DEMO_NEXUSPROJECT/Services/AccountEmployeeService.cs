using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface AccountEmployeeService
{
    public bool login(string email, string password, string role);
    public bool create(Employee employee);
    public Employee findByUsername(string username);
    public List<Employee> findAll();
    public int caculateEmployee();
	public bool update(Employee employee);
    public int CountCustomers();
    public int CountProducts();
    public int CountOrders();
}
