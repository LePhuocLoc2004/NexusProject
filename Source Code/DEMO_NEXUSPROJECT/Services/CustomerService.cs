using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface CustomerService
{
	public int? login(string fullname, string password);
	public bool create(Customer customer);
	public bool update(Customer customer);
    public Customer findByPhoneNumber(string fullname);
     Task<List<Customer>> GetAllCustomersAsync(int n);
    Task<Customer?> GetCustomerDetailsAsync(int customerId);
    public int caculateCustomer();
    public Customer findByEmail(string email);
    public Customer creteCustomer(int customerId, string connectionType);
    public Customer finbyId(int customerId);
    public List<Customer> findAll();
    //
    //

    public IEnumerable<Customer> GetAllCustomers();

    public Customer GetCustomerById(int customerId);

    List<Invoice> GetOutstandingFees(int customerId);

    public double CalculatePenaltyFee(double amount, DateTime issueDate);

    public List<Customer> findByKeyword(string keyword);

    public List<CustomerFeedback> findByKeywords(string keyword);
}
