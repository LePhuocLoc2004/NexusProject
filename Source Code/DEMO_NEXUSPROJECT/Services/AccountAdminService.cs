using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface AccountAdminService
{
    public bool create(Employee employee);
    
    public bool login(string email, string password, string role);

    //
    public Employee findByUsername(string username);

    public bool update(Employee employee);

    Employee GetEmployeeById(int id);

}
