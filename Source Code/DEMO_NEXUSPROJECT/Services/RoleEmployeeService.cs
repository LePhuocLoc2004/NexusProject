using DEMO_NEXUSPROJECT.Models;
namespace DEMO_NEXUSPROJECT.Services;

public interface RoleEmployeeService
{
    public List<Role> findAll();
    public Role findById(int id);
}
