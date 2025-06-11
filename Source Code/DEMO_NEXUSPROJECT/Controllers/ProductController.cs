using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DEMO_NEXUSPROJECT.Controllers;
[Route("product")]
public class ProductController : Controller
{
    private ServicePackageService servicePackageService;
    private PaymentService paymentService;
    private OrdersService ordersService;
    private InvoiceService invoiceService;
    private CustomerService customerService;
    private IConfiguration configuration;
    private FinanceService financeService;
    private ConnectionsServices connectionsServices;

    private ConnectionPackageService connectionPackageService;
    private ConnectionRequestService connectionRequestService;
    private TransactionLogService transactionLogService;

    public ProductController(ServicePackageService _servicePackageService,
        IConfiguration configuration, PaymentService _paymentService, OrdersService _ordersService,
        InvoiceService _invoiceService, CustomerService _customerService, ConnectionsServices _connectionsServices, FinanceService financeService,
        ConnectionPackageService connectionPackageService, ConnectionRequestService connectionRequestService, TransactionLogService transactionLogService
        )
    {
        servicePackageService = _servicePackageService;
        this.configuration = configuration;
        this.paymentService = _paymentService;
        this.ordersService = _ordersService;
        this.invoiceService = _invoiceService;
        this.customerService = _customerService;
        this.connectionsServices = _connectionsServices;
        this.financeService = financeService;
        this.connectionPackageService = connectionPackageService;
        this.transactionLogService = transactionLogService;
        this.connectionRequestService = connectionRequestService;
    }
    [HttpGet]
    [Route("product")]
    public IActionResult Product()
    {
        ViewBag.servicepackage = servicePackageService.findAll();
        var latestProducts = servicePackageService.latestProduct(6);
        ViewBag.LaterA = latestProducts;
        if (latestProducts.Any())
        {
            int lastId = latestProducts.Last().ServicePackageId;
            ViewBag.LaterB = servicePackageService.GetNextProducts(lastId, 3);
        }
        else
        {
            ViewBag.LaterB = new List<ServicePackage>();
        }
        return View("Product");
    }

    [Route("details")]
    public IActionResult Details(int id)
    {
        ViewBag.servicePackageService = servicePackageService.findById(id);
        ViewBag.servicePackageServices = servicePackageService.ralatedServicePackage(id, 4);
        return View("Details");
    }

    [Route("checkout")]
    public IActionResult Checkout()
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId");
        if (customerId == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }

        var customer = customerService.finbyId(customerId.Value); // Find customer by ID
        if (customer == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }

        if (HttpContext.Session.GetString("product") != null)
        {
            var product = JsonConvert.DeserializeObject<List<ServicePackage>>(HttpContext.Session.GetString("product"));
            if (product.Any())
            {
                ViewBag.cart = product;
                ViewBag.total = product.Sum(i => i.Price);
            }
            else
            {
                ViewBag.cart = new List<ServicePackage>();
                ViewBag.total = 0;
            }
        }
        else
        {
            ViewBag.cart = new List<ServicePackage>();
            ViewBag.total = 0;
        }

        return View("Checkout", customer);
    }


    [Route("buy/{id}")]
    public IActionResult Buy(int id)
    {
        var servicePackage = servicePackageService.findById(id);
        List<ServicePackage> product;
        if (HttpContext.Session.GetString("product") == null)
        {
            product = new List<ServicePackage>();
        }
        else
        {
            product = JsonConvert.DeserializeObject<List<ServicePackage>>(HttpContext.Session.GetString("product"));
        }
        var index = product.FindIndex(sp => sp.ServicePackageId == id);
        if (index == -1)
        {
            product.Add(servicePackage);
        }
        else
        {
            product[index] = servicePackage;
        }
        HttpContext.Session.SetString("product", JsonConvert.SerializeObject(product));

        return RedirectToAction("Checkout");
    }

    [HttpPost]
    [Route("ProcessPayment")]
    public IActionResult ProcessPayment(string paymentMethod, Customer updatedCustomerInfo)
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId");

        if (customerId == null)
        {
            return RedirectToAction("LoginRegister", "account");
        }
        var cartJson = HttpContext.Session.GetString("product");
        if (string.IsNullOrEmpty(cartJson))
        {
            TempData["Msg"] = "Your cart is empty.";
            return RedirectToAction("Checkout");
        }
        var cart = JsonConvert.DeserializeObject<List<ServicePackage>>(cartJson);
        if (cart == null || !cart.Any())
        {
            TempData["Msg"] = "Your cart is empty.";
            return RedirectToAction("Checkout");
        }

        // Lấy thông tin khách hàng từ cơ sở dữ liệu
        var customer = customerService.finbyId(customerId.Value);
        if (customer == null)
        {
            return RedirectToAction("LoginRegister", "account");
        }

        // Cập nhật thông tin khách hàng nếu cần
        if (!string.IsNullOrEmpty(updatedCustomerInfo.FullName))
        {
            customer.FullName = updatedCustomerInfo.FullName;
        }
        if (!string.IsNullOrEmpty(updatedCustomerInfo.Email))
        {
            customer.Email = updatedCustomerInfo.Email;
        }
        if (!string.IsNullOrEmpty(updatedCustomerInfo.PhoneNumber))
        {
            customer.PhoneNumber = updatedCustomerInfo.PhoneNumber;
        }
        if (!string.IsNullOrEmpty(updatedCustomerInfo.Address))
        {
            customer.Address = updatedCustomerInfo.Address;
        }
        if (!string.IsNullOrEmpty(updatedCustomerInfo.CityCode))
        {
            customer.CityCode = updatedCustomerInfo.CityCode;
        }
        if (!string.IsNullOrEmpty(updatedCustomerInfo.Idcard))
        {
            customer.Idcard = updatedCustomerInfo.Idcard;
        }
        // Cập nhật thông tin khách hàng trong cơ sở dữ liệu
        customerService.update(customer);


        var Citycode = JsonConvert.DeserializeObject<List<Customer>>(cartJson);
        if (Citycode == null || !Citycode.Any())
        {
            TempData["Msg"] = "Your cart is empty.";
            return RedirectToAction("Profile", "account");
        }
        string status = paymentMethod == "PayPal" ? "Paid" : "Pending";
        var invoice = invoiceService.creteInvoice(customerId.Value, cart.Sum(item => item.Price), status);
        var order = ordersService.createOrder(customerId.Value, cart.First().ConnectionType);
        var accountNumber = customerService.creteCustomer(customerId.Value, cart.First().ConnectionType);
        var connection = connectionsServices.CreateConnection(customerId.Value, cart.First().ConnectionType, cart.First().ServicePackageId);
        var totalAmount = cart.Sum(item => item.Price);
        double paymentAmount = totalAmount; // Tổng số tiền thanh toán
        var finance = financeService.CreateFinance(customerId.Value, connection.ConnectionId, totalAmount, paymentMethod, paymentAmount);

        var connectionPackage = connectionPackageService.CreateConnectionPackage(connection.ConnectionId, cart.First().ServicePackageId, connection.TerminationDate);
        var connectionRequest = connectionRequestService.CreateConnectionRequest(customerId.Value, cart.First().ConnectionType);
        var transactionDetails = new Dictionary<string, object>
        {
        {"ConnectionPackage StartDate", connectionPackage.StartDate},
        {"ConnectionPackage Code", connectionPackage.EndDate},
        {"Order Code", order.OrderCode},
        {"Order Connection Type", order.ConnectionType},
        {"Order Status", order.Status},
        {"Order Date", order.OrderDate.ToString("yyyy-MM-dd")},
        {"Invoice Number", invoice.InvoiceNumber},
        {"Invoice Amount", invoice.Amount.ToString("C")},
        {"Invoice Issue Date", invoice.IssueDate.ToString("yyyy-MM-dd")},
        {"Invoice Status", invoice.Status},
        {"Connection Account Number", connection.AccountNumber},
        {"Connection Status", connection.Status},
        {"Connection Activation Date", connection.ActivationDate.ToString("yyyy-MM-dd")},
        {"Connection Termination Date", connection.TerminationDate.ToString("yyyy-MM-dd")},
        {"ServicePackageName ", cart.First().ServicePackageName},
        {"Price", cart.First().Price.ToString("C")},
        {"ConnectionRequest RequestDate", connectionRequest.RequestDate},
        {"ConnectionRequest Status", connectionRequest.Status},
        {"Finace TotalAmount", finance.TotalAmount},
        {"Finace AmountPaid", finance.AmountPaid},
        {"Finace RemainingAmount", finance.RemainingAmount},

        };

        var transactionDetailsString = JsonConvert.SerializeObject(transactionDetails);
        var transactionLog = transactionLogService.CreateTransactionLog(customerId.Value, "Purchase", DateTime.Now, transactionDetailsString);
        if (paymentMethod == "PayPal")
        {
            var paypalUrl = configuration["PayPal:PostUrl"];
            var business = configuration["PayPal:Business"];
            var returnUrl = configuration["PayPal:ReturnUrl"];

            var query = new QueryBuilder();
            query.Add("cmd", "_cart");
            query.Add("upload", "1");
            query.Add("business", business);
            query.Add("return", returnUrl);
            query.Add("custom", invoice.InvoiceId.ToString());
            int index = 1;
            foreach (var item in cart)
            {
                query.Add($"item_number_{index}", item.ServicePackageId.ToString());
                query.Add($"item_name_{index}", item.ServicePackageName);
                query.Add($"amount_{index}", item.Price.ToString());
                index++;
            }
            HttpContext.Session.SetString("product", JsonConvert.SerializeObject(new List<ServicePackage>()));

            return Redirect($"{paypalUrl}{query.ToQueryString()}");
        }
        else if (paymentMethod == "PayLater")
        {
            var payment = paymentService.createPayment(invoice.InvoiceId, invoice.Amount, paymentMethod);
            HttpContext.Session.SetString("product", JsonConvert.SerializeObject(new List<ServicePackage>()));
            return RedirectToAction("OrderConfirmation", new { orderCode = order.OrderCode });
        }
        else
        {
            TempData["Msg"] = "Invalid payment method.";
            return RedirectToAction("Checkout");
        }
    }   

    [HttpGet]
    [Route("OrderConfirmation")]
    public IActionResult OrderConfirmation(string payer_id, string payment_status, string txn_id, string mc_gross, string payment_date, string first_name, string last_name, string item_name1, string item_number1, string quantity1, string custom)
    {
        if (payment_status != "Completed")
        {
            TempData["Msg"] = "Payment Success";
            return RedirectToAction("Checkout");
        }
        var customerId = HttpContext.Session.GetInt32("CustomerId");
        if (customerId == null)
        {
            TempData["Msg"] = "Please login to complete the purchase.";
            return RedirectToAction("LoginRegister", "Product");
        }

        if (string.IsNullOrEmpty(mc_gross) || !double.TryParse(mc_gross, out double grossAmount))
        {
            TempData["Msg"] = "Invalid payment amount.";
            return RedirectToAction("Checkout");
        }

        if (string.IsNullOrEmpty(custom) || !int.TryParse(custom, out int invoiceId))
        {
            TempData["Msg"] = "Invalid invoice ID.";
            return RedirectToAction("Checkout");
        }

        try
        {
            var invoice = invoiceService.findByID(invoiceId);
            var payment = paymentService.createPayment(invoice.InvoiceId, grossAmount, "PayPal");
            ViewBag.PayerID = payer_id;
            ViewBag.TransactionID = txn_id;
            ViewBag.GrossAmount = grossAmount;
            ViewBag.PaymentDate = payment_date;
            ViewBag.FirstName = first_name;
            ViewBag.LastName = last_name;
            ViewBag.ItemName = item_name1;
            ViewBag.ItemNumber = item_number1;
            ViewBag.Quantity = quantity1;
            ViewBag.PackageName = item_name1;
            return View("OrderConfirmation");
        }
        catch (Exception ex)
        {
            TempData["Msg"] = $"An error occurred while processing your order: {ex.Message}";
            return RedirectToAction("Checkout");
        }
    }
}
