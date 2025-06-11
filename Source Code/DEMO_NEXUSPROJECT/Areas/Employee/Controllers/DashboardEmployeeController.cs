using Microsoft.AspNetCore.Mvc;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using System.Security.Claims;

namespace DEMO_NEXUSPROJECT.Areas.Employee.Controllers
{
    [Area("employee")]
    [Route("employee/dashboard")]
    [Route("dashboard")]
    public class DashboardEmployeeController : Controller
    {
        private readonly AccountEmployeeService accountService;

        public DashboardEmployeeController(AccountEmployeeService _accountService)
        {
            accountService = _accountService;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            int customerCount = accountService.CountCustomers();
            int productCount = accountService.CountProducts();
            int orderCount = accountService.CountOrders();
            ViewBag.CustomerCount = customerCount;
            ViewBag.ProductCount = productCount;
            ViewBag.OrderCount = orderCount;
            return View("Index");
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                // Handle case where username is not found in session (e.g., redirect to login page)
                return RedirectToAction("Index", "LoginEmployee", new { Area = "employee" });
            }

            var account = accountService.findByUsername(username);
            if (account == null)
            {
                // Handle case where account is not found
                return NotFound();
            }

            return View("Profile", account);
        }


        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(DEMO_NEXUSPROJECT.Models.Employee employee)
        {
            var currentAcc = accountService.findByUsername(HttpContext.Session.GetString("username"));
            if (!string.IsNullOrEmpty(employee.Password))
            {
                currentAcc.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password); 
            }
            currentAcc.Photo= employee.Photo;
            currentAcc.Email = employee.Email; 
            currentAcc.FullName = employee.FullName;
            currentAcc.PhoneNumber = employee.PhoneNumber; 

            accountService.update(currentAcc); 

            return View("Profile", currentAcc); 
        }
    }
}
