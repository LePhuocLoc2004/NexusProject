using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_NEXUSPROJECT.Areas.Admin.Controllers;
[Area("admin")]
[Route("admin/dashboard")]
public class DashboardAdminController : Controller
{
    private InvoiceService invoiceService;
    private CustomerService customerService;
    private AccountEmployeeService accountEmployeeService;
    private RetailstoreService retailstoreService;
    private OrderService orderService;
    
    public DashboardAdminController(InvoiceService _invoiceService, CustomerService customerService, AccountEmployeeService accountEmployeeService,
        RetailstoreService retailstoreService, OrderService _orderService)
    {
        invoiceService = _invoiceService;
        this.customerService = customerService;
        this.accountEmployeeService = accountEmployeeService;
        this.retailstoreService = retailstoreService;
        this.orderService = _orderService;

    }
    [Route("index")]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var customers =  await customerService.GetAllCustomersAsync(4);
        var totalRetaisStore = retailstoreService.caculateRetailsStore();
        var totalEmployee = accountEmployeeService.caculateEmployee();
        var totalCustomer = customerService.caculateCustomer();
        var totalAmount = invoiceService.CalculateTotalAmount();
        ViewBag.totalRetaisStore = totalRetaisStore;
        ViewBag.totalEmployee = totalEmployee;
        ViewBag.totalCustomer = totalCustomer;
        ViewBag.TotalAmount = totalAmount;
        ViewBag.Orrder = orderService.latestOrder(4);
        ViewBag.customers = customers;
        return View(totalAmount);
    }




    [Route("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var customer = await customerService.GetCustomerDetailsAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }
}
