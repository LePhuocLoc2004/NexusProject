using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class ProductEmployeeServiceImpl : ProductEmployeeService
{
    private DatabaseContext db;
    public ProductEmployeeServiceImpl(DatabaseContext _db)
    {
        db = _db;

    }

    public bool create(Product product)
    {
        try
        {
            db.Products.Add(product);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public bool delete(int productId)
    {
        try
        {
            db.Products.Remove(db.Products.Find(productId));
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<Product> findAll()
    {
        return db.Products.ToList();
    }

    public Product findById(int productId)
    {
        return db.Products.Find(productId);
    }

    public bool update(Product product)
    {
        try
        {
            db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }
}
