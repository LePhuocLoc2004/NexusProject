using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class RetailstoreServiceImpl : RetailstoreService
{
    private DatabaseContext db;
    public RetailstoreServiceImpl(DatabaseContext db)
    {
        this.db = db;
    }

    public int caculateRetailsStore()
    {
        return db.RetailStores.Count();
    }

    public bool create(RetailStore store)
    {
        try
        {
            db.RetailStores.Add(store);
            return db.SaveChanges() > 0;
        }
        catch
        {
            return false;
        }

    }

    public List<RetailStore> findAll()
    {
       return db.RetailStores.ToList();
    }
}
