using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DEMO_NEXUSPROJECT.Controllers;
[Route("account")]
public class AccountController : Controller
{
    private CustomerService customerService;
    private MailServices mailServices;
    private TransactionLogService transactionLogService;
    private IConfiguration configuration;
    private IWebHostEnvironment webHostEnvironment;
    private OrderService orderService;
    public AccountController(CustomerService _customerService, IConfiguration _configuration, MailServices _mailServices, IWebHostEnvironment webHostEnvironment, OrderService orderService, TransactionLogService transactionLogService)
    {
        customerService = _customerService;
        configuration = _configuration;
        mailServices = _mailServices;
        this.webHostEnvironment = webHostEnvironment;
        this.orderService = orderService;
        this.transactionLogService = transactionLogService;
    }

    [HttpGet]
    [Route("login")]
    [Route("register")]
    public IActionResult LoginRegister()
    {
        var customer = new Customer();
        customer.RegistrationDate = DateTime.Now;
        return View("LoginRegister", customer);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(string fullname, string password)
    {
        var customerId = customerService.login(fullname, password);
        if (customerId != null)
        {
            HttpContext.Session.SetInt32("CustomerId", customerId.Value);
            return RedirectToAction("Home", "Home");
        }
        else
        {
            TempData["Msg"] = "Login failed";
            return RedirectToAction("LoginRegister");
        }
        
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register( Customer customer)
    {
        customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
        customer.Status = true;
        customer.SecurityCode = RandomHelpers.generteSecurityCode();
        customer.RegistrationDate = DateTime.Now;
        customer.AccountNumber = null;
        customer.Idcard = null;
        customer.Address = null;
        customer.Photo = null;
        customer.CityCode = null;
        if (customerService.create(customer))
        {
            var url = configuration["BaseUrl"] + "account/verify/" + customer.FullName + "/" + customer.SecurityCode;
            var body = " Nhấn vào <a href='" + url + "'>đây</a> để kích hoạt tài khoản.";
            if (mailServices.Send("ha11072004cc@gmail.com", customer.Email, "Verify", body))
            {
                return RedirectToAction("LoginRegister");
            }
            else
            {
                TempData["Msg"] = "Gửi email kích hoạt thất bại.";
                return RedirectToAction("LoginRegister");
            }
        }
        else
        {
            TempData["Msg"] = "Registration failed.";
            return RedirectToAction("LoginRegister");
        }
    }

    [Route("verify/{fullname}/{securitycode}")]
	public IActionResult Verify(string fullname, string securitycode)
    {
        var customer = customerService.findByPhoneNumber(fullname);
        if (customer == null)
        {
            ViewBag.Msg = "tai khoan khong ton tai";
        }
        else
        {
            if (customer.SecurityCode == securitycode)
            {
                customer.Status = true;
                if (customerService.update(customer))
                {
                    ViewBag.Msg = "kich hoat tai khoan thanh cong";
                }
                else
                {
                    ViewBag.Msg = "kich hoat tai khoan that bai";
                }
            }
            else
            {
                ViewBag.Msg = "kich hoat tai khoan that bai";
            }
        }
        return View("LoginRegister");
    }

    [HttpGet]
    [Route("profile")]
    public IActionResult Profile()
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
        return View("Profile", customer);
    }

    [HttpPost]
    [Route("profile")]
    public IActionResult Profile(Customer customer, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var fileName = FileHelper.generateFileName(file.FileName);
            var path = Path.Combine(webHostEnvironment.WebRootPath, "img", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            customer.Photo = fileName;
            Debug.WriteLine("Photo: " + customer.Photo);
        }
        else
        {
            customer.Photo = "logo1.png";
        }

        var customerId = HttpContext.Session.GetInt32("CustomerId");
        if (customerId == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }
        var customerIdacc = customerService.finbyId(customerId.Value);
        if (customer == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }
        if (customerIdacc == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }

        if (!string.IsNullOrEmpty(customer.Password))
        {
            customerIdacc.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
        }
        customerIdacc.AccountNumber = customer.AccountNumber;
        customerIdacc.Email = customer.Email;
        customerIdacc.FullName = customer.FullName;
        customerIdacc.PhoneNumber = customer.PhoneNumber;
        customerIdacc.Address = customer.Address;
        customerIdacc.Idcard = customer.Idcard;
        customerIdacc.CityCode = customer.CityCode;
        customerIdacc.Photo = customer.Photo;
        if (customerService.update(customerIdacc))
        {
            TempData["Msg"] = "Update Success";
            return RedirectToAction("profile");

        }
        else
        {
            TempData["Msg"] = "Update failed";
            return View("Profile", customer);
        }
    }

    [HttpGet]
    [Route("forgetpassword")]
    public IActionResult Forgetpassword()
    {

        return View("forgetpassword");
    }
    [HttpPost]
    [Route("forgetpassword")]
    public IActionResult Forgetpassword(string email)
    {
        var customer = customerService.findByEmail(email);
        if (customer == null)
        {
            TempData["Msg"] = "Email khong hop le";
            return RedirectToAction("forgetpassword");
        }
        else
        {
            customer.SecurityCode = RandomHelpers.generteSecurityCode();
            if (customerService.update(customer))
            {
                // gui email
                var url = configuration["BaseUrl"] + "account/resetpassword/" + customer.FullName + "/" + customer.SecurityCode;
                var body = " Nhan vao <a href='" + url + "'>day</a> de reset password tai khoan.";
                if (mailServices.Send("ha11072004cc@gmail.com", customer.Email, "Reset  Password", body))
                {
                    return View("Message");
                }
                else
                {
                    TempData["Msg"] = "gui Email reset that bai";
                    return RedirectToAction("forgetpassword");
                }
            }
            else
            {
                TempData["Msg"] = "Email khong hop le";
                return RedirectToAction("forgetpassword");
            }
        }
    }




    [HttpGet]
    [Route("resetpassword/{fullname}/{securitycode}")]
    public IActionResult Resetpassword(string fullname, string securitycode)
    {
        var customer = customerService.findByPhoneNumber(fullname);
        if (customer == null)
        {
            ViewBag.Msg = "tai khoan khong ton tai";
            return View("Message2");
        }
        else
        {
            if (customer.SecurityCode == securitycode)
            {
                ViewBag.fullname = fullname;
                ViewBag.securityCode = securitycode;
                return View("Resetpassword");
            }
            else
            {
                ViewBag.Msg = "Reset password cho  tai khoan that bai";
                return View("Message2");
            }
        }

    }
    [HttpPost]
    [Route("resetpassword/{fullname}/{securitycode}")]
    public IActionResult Resetpassword(string password, string repassword, string fullname, string securitycode)
    {
        if (password != repassword)
        {
            TempData["Msg"] = "password khong hop le";
            return RedirectToAction("Resetpassword", new { username = fullname, securitycode = securitycode });
        }
        else
        {
            var customer = customerService.findByPhoneNumber(fullname);
            customer.Password = BCrypt.Net.BCrypt.HashPassword(password);
            if (customerService.update(customer))
            {
                TempData["Msg"] = "cap nhap thanh cong";
                return RedirectToAction("login","account");
            }
            else
            {
                TempData["Msg"] = "cap nhap that bai";
                return RedirectToAction("Resetpassword", new { username = fullname, securitycode = securitycode });
            }
        }

    }


    [Route("detailsinvoice")]
    public IActionResult Detailsinvoice()
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

        var orders = orderService.findAll(customerId.Value);
        var viewModel = new Customer
        {
            CustomerId = customerId.Value,
            Orders = orders,
        };
        return View(viewModel);
    }
    [Route("detailstransactionlog")]
    public IActionResult DetailsTransactionLog()
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId");
        if (customerId == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }

        var customer = customerService.finbyId(customerId.Value); // Tìm thông tin khách hàng
        if (customer == null)
        {
            return RedirectToAction("LoginRegister", "Account");
        }

        var transactionLogs = transactionLogService.findbyId(customerId.Value); // Lấy danh sách transaction logs

        var viewModel = new Customer
        {
            CustomerId = customerId.Value,
            TransactionLogs = transactionLogs,
        };

        return View(viewModel);
    }

}
