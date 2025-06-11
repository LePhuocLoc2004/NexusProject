    using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_NEXUSPROJECT.Areas.Admin.Controllers;
[Area("admin")]
[Route("admin/login")]
public class LoginAdminController : Controller
{

    private AccountAdminService accountadmintService;
    private IConfiguration configuration;
    public LoginAdminController(AccountAdminService _accountadmintService, IConfiguration configuration)
    {
        accountadmintService = _accountadmintService;
        this.configuration = configuration;
    }
    [HttpGet]
    [Route("")]
    [Route("index")]
    [Route("~/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Route("index")]
    public IActionResult Index(string email, string password)
    {
        if (accountadmintService.login(email, password, "Admin"))
        {
            return RedirectToAction("Index", "dashboard", new { Area = "admin" });
        }
        else
        {
            TempData["Msg"] = "InvaLid";
            return RedirectToAction("Index", "login", new { Area = "admin" });
        }
    }

    [HttpGet]
    [Route("register")]
    public IActionResult Register()
    {

        return View("Register");
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(string username, string email, string password, string fullName, string role)
    {
        var employee = new Models.Employee
        {
            Username = username,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = fullName,
            Role = role,
            Status = true,
            SecurityCode = RandomHelpers.generteSecurityCode(),
            PhoneNumber = "", 
            Photo = "" 
        };

        if (accountadmintService.create(employee))
        {
            TempData["Msg"] = "Registration successful.";
            return RedirectToAction("Index", "login");
        }
        else
        {
            TempData["Msg"] = "Registration failed.";
            return RedirectToAction("Register");
        }
    }
}

