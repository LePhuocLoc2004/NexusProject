using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class AccountEmployeeServiceImpl : AccountEmployeeService
{
    private DatabaseContext db;
    public AccountEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;

    }

    public int CountCustomers()
    {
        return db.Customers.Count();
    }

    public int CountProducts()
    {
        return db.Products.Count();
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
        var employee = db.Employees.SingleOrDefault(e => e.Email == email && e.Status);
        if (employee != null)
        {
            if (BCrypt.Net.BCrypt.Verify(password, employee.Password))
            {
                foreach (var rol in employee.Roles)
                {
                    if (rol.RoleName == role)
                    {
                        return true;
                    }
                }
            }
            return BCrypt.Net.BCrypt.Verify(password, employee.Password);
        }
        return false;
    }
    public Employee findByUsername(string username)
    {
        return db.Employees.FirstOrDefault(e => e.Username == username);
    }

    public List<Employee> findAll()
    {
        return db.Employees.ToList();
    }

	public bool update(Employee employee)
	{
		try
		{
			db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			return db.SaveChanges() > 0;
		}
		catch
		{
			return false;
		}
	}

    public int CountOrders()
    {
        return db.Orders.Count();
    }

    public int caculateEmployee()
    {
        return db.Employees.Count();
    }
}
