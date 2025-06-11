using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface ProductEmployeeService
{
    public List<Product> findAll();
    public bool update(Product product);
    public bool delete(int productId);
    public Product findById(int productId);
    public bool create(Product product);
}
