using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class CustomerEmployeeServiceImpl : CustomerEmployeeService
{
    private DatabaseContext db;
    public CustomerEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;

    }

    public bool create(Customer customer)
    {
        try
        {
            db.Customers.Add(customer);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool delete(int customerId)
    {
        try
        {
            db.Customers.Remove(db.Customers.Find(customerId));
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public Customer findById(int customerId)
    {
        return db.Customers .Find(customerId);
    }

    public List<Customer> findAll()
    {
        return db.Customers.ToList();
    }

    public bool update(Customer customer)
    {
        try
        {
            db.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
    public int CountCustomers()
    {
        return db.Customers.Count();
    }
}
