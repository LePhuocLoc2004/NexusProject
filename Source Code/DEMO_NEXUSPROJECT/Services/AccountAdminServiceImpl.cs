using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class AccountAdminServiceImpl : AccountAdminService
{
    private DatabaseContext db;

    public AccountAdminServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }
    public bool create(Employee employee)
    {
        try
        {
            db.Employees.Add(employee);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool login(string email, string password, string role)
    {
        var admin = db.Employees.SingleOrDefault(a => a.Email == email && a.Status);
        if(admin != null) 
        {
            if (!BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                return false;
            }
            foreach (var rol in admin.Roles)
            {
                if (rol.RoleName == role)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    public Employee findByUsername(string username)
    {
        return db.Employees.SingleOrDefault(a => a.Username == username);
    }


    public bool update(Employee employee)
    {
        try
        {
            db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public Employee GetEmployeeById(int id)
    {
        return db.Employees.Find(id);
    }
}
