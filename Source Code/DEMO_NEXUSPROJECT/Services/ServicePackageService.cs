using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface ServicePackageService
{
    public List<ServicePackage> GetNextProducts(int startId, int n);
    public List<ServicePackage> latestProduct(int n);
    public List<ServicePackage> findAll();
    public bool created(ServicePackage servicePackage);
    public bool Update(ServicePackage servicePackage);
    public ServicePackage findById(int servicePackageId);
    public int exist(int id, List<ServicePackage> cart);
    public List<ServicePackage> ralatedServicePackage(int servicePackageId, int n);

    IEnumerable<ServicePackage> GetServicePackagesPaged(int page, int pageSize);
    int GetTotalServicePackages();
}
