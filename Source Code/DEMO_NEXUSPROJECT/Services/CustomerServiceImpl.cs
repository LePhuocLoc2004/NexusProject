using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DEMO_NEXUSPROJECT.Services;

public class CustomerServiceImpl : CustomerService
{
	private readonly DatabaseContext db;

	public CustomerServiceImpl(DatabaseContext _db)
	{
		db = _db;
	}

	public bool create(Customer customer)
	{
		try
		{
			db.Customers.Add(customer);
			db.SaveChanges();
			return true;
		}catch (Exception ex)
		{
			return false;
		}
	}

    public Customer creteCustomer(int customerId, string connectionType)
    {
        try
        {
            var customer1 = db.Customers.SingleOrDefault(c => c.CustomerId == customerId);
            if (customer1 == null)
            {
                throw new Exception($"Customer with ID {customerId} not found.");
            }
            var cityCode = customer1.CityCode;
            var customerCode = GenerateOrderCode.GenerateOrderCode3(connectionType, cityCode);
            customer1.AccountNumber = customerCode;
            db.Customers.Update(customer1);
            db.SaveChanges();

            return customer1;
        }
        catch (Exception ex)
        { 
            Debug.WriteLine($"Error occurred while updating customer AccountNumber: {ex.Message}");
            throw;
        }
    }

    public Customer findByEmail(string email)
    {
        return db.Customers.SingleOrDefault(cus => cus.Email == email);
    }

    public Customer findByPhoneNumber(string fullname)
    {
        return db.Customers.SingleOrDefault(cus => cus.FullName == fullname);
    }

    public Customer finbyId(int customerId)
    {
        return db.Customers.SingleOrDefault(cus => cus.CustomerId == customerId);
    }

    public int? login(string fullname, string password)
	{
        var customer = db.Customers.SingleOrDefault(cus => cus.FullName == fullname && cus.Status);
        if (customer != null && BCrypt.Net.BCrypt.Verify(password, customer.Password))
        {
            return customer.CustomerId;
        }
        return null;
    }

	public bool update(Customer customer)
	{
		try
		{
			db.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			db.SaveChanges();
			return true;
		}catch (Exception ex)
		{
			return false;
		}
	}
    //
    public IEnumerable<Customer> GetAllCustomers()
    {
        return db.Customers
                 .Where(c => c.Invoices.Any(i => i.Status == "Unpaid"))
                 .ToList();
    }

    public Customer GetCustomerById(int customerId)
    {
        return db.Customers.Find(customerId);
    }

    public List<Invoice> GetOutstandingFees(int customerId)
    {
        var outstandingFees = db.Invoices
            .Where(i => i.CustomerId == customerId && i.Status == "  ")
            .Select(i => new Invoice
            {
                InvoiceId = i.InvoiceId,
                Amount = i.Amount,
                IssueDate = i.IssueDate,
                Status = i.Status.ToString()
            })
            .ToList();

        return outstandingFees;
    }


    public double CalculatePenaltyFee(double amount, DateTime issueDate)
    {
        // Calculate overdue days
        var overdueDays = (DateTime.Now - issueDate).Days - 30;

        // If overdue, calculate penalty fee
        if (overdueDays > 0)
        {
            // Calculate penalty fee (assuming 1% per day)
            double penaltyRate = 0.01; // 1% penalty rate per day
            double penaltyFee = amount * penaltyRate * overdueDays;

            return penaltyFee;
        }

        // No overdue, return 0 penalty fee
        return 0;
    }


    public List<Customer> findByKeyword(string keyword)
    {
        return db.Customers.Where(p => p.FullName.Contains(keyword)).ToList();
    }

    public List<CustomerFeedback> findByKeywords(string keyword)
    {
        return db.CustomerFeedbacks.Where(p => p.Customer.FullName.Contains(keyword)).ToList();
    }

    public int caculateCustomer()
    {
        return db.Customers.Count();
    }

    public async Task<List<Customer>> GetAllCustomersAsync(int n)
    {
        return db.Customers.OrderByDescending(p => p.CustomerId).Take(n)
                 .AsNoTracking() // Sử dụng AsNoTracking để tăng tốc độ truy vấn
                 .ToList();      // Sử dụng ToList để đảm bảo làm việc đồng bộ khi không cần thiết phải làm việc bất đồng bộ
    }

    public async Task<Customer?> GetCustomerDetailsAsync(int customerId)
    {
        return db.Customers
                 .AsNoTracking() // Sử dụng AsNoTracking để tăng tốc độ truy vấn
                 .Include(c => c.ConnectionRequests)
                 .Include(c => c.Connections)
                 .Include(c => c.CustomerFeedbacks)
                 .Include(c => c.CustomerServicePackages)
                 .Include(c => c.Finances)
                 .Include(c => c.Invoices)
                 .Include(c => c.Orders)
                 .Include(c => c.TransactionLogs)
                 .FirstOrDefault(c => c.CustomerId == customerId);
    }

    public List<Customer> findAll()
    {
        return db.Customers
                 .Where(c => c.Invoices.Any(i => i.Status == "Unpaid"))
                 .ToList();
    }
}
