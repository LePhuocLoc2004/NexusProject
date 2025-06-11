using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class RoleEmployeeServiceImpl : RoleEmployeeService
{
    private DatabaseContext db;

    public RoleEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public List<Role> findAll()
    {
        return db.Roles.ToList();
    }

    public Role findById(int id)
    {
        return db.Roles.Find(id);
    }
}
