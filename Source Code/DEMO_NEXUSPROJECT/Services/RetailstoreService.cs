using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface RetailstoreService
{
    public List<RetailStore> findAll();
    public bool create(RetailStore store);

    public int caculateRetailsStore();
}
