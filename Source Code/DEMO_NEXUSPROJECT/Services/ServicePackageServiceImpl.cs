using DEMO_NEXUSPROJECT.Models;
using Microsoft.EntityFrameworkCore;

namespace DEMO_NEXUSPROJECT.Services;

public class ServicePackageServiceImpl : ServicePackageService
{
    private DatabaseContext db;
    public ServicePackageServiceImpl(DatabaseContext db)
    {
        this.db = db;
    }

    public bool created(ServicePackage servicePackage)
    {
        try
        {
            db.ServicePackages.Add(servicePackage);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<ServicePackage> findAll()
    {
        return db.ServicePackages.ToList();
    }

    public ServicePackage findById(int servicePackageId)
    {
        return db.ServicePackages.Find(servicePackageId);
    }

    public List<ServicePackage> latestProduct(int n)
    {
        return db.ServicePackages.OrderByDescending(p => p.ServicePackageId).Take(n).ToList();
    }

    public bool Update(ServicePackage servicePackage)
    {
        try
        {
            db.Entry(servicePackage).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }
    }

    public List<ServicePackage> GetNextProducts(int startId, int n)
    {
        return db.ServicePackages.Where(p => p.ServicePackageId < startId).OrderByDescending(p => p.ServicePackageId).Take(n).ToList();
    }

    public List<ServicePackage> ralatedServicePackage(int servicePackageId, int n)
    {
        var product = findById(servicePackageId);
        return db.ServicePackages.Where(p => p.ServicePackageId != servicePackageId).Take(n).ToList();
    }

    public int exist(int id, List<ServicePackage> cart)
    {
        for (var i = 0; i < cart.Count; i++)
        {
            if (cart[i].ServicePackageId == id)
            {
                return i;
            }

        }
        return -1;
    }

    public IEnumerable<ServicePackage> GetServicePackagesPaged(int page, int pageSize)
    {
        return db.ServicePackages
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();
    }

    public int GetTotalServicePackages()
    {
        return db.ServicePackages.Count();
    }

    
}
