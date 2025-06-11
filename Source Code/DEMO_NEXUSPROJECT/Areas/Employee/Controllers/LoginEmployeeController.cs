using Microsoft.AspNetCore.Mvc;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using DEMO_NEXUSPROJECT.Helpers;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DEMO_NEXUSPROJECT.Areas.Employee.Controllers;
[Area("employee")]
[Route("employee/Login")]
[Route("employee")]
public class LoginEmployeeController : Controller
{
    private AccountEmployeeService accountService;
    private RoleEmployeeService roleService;
    private IWebHostEnvironment webHostEnvironment;
    public LoginEmployeeController(AccountEmployeeService _accountService, RoleEmployeeService _roleService, IWebHostEnvironment _webHostEnvironment)
    {
        accountService = _accountService;
        roleService = _roleService;
        webHostEnvironment = _webHostEnvironment;
    }

    [Route("index")]
    [Route("")]
    [Route("~/")]
    public IActionResult Index()
    {

        return View();
    }

    [HttpPost]
    [Route("index")]
    public IActionResult Index(string username, string password,string role)
    {
        var employee = accountService.findByUsername(username);
        if (accountService.login(username, password, "Employee"))
        {
            
            HttpContext.Session.SetString("username",username);
            HttpContext.Session.SetString("userphoto", employee.Photo);

            return RedirectToAction("Index", "DashboardEmployee", new { Area = "Employee" });
        }
        else
        {
            TempData["Msg"] = "Invalid";
            return RedirectToAction("Index", "LoginEmployee", new { Area = "Employee" });
        }
    }

    [Route("logout")]
    public IActionResult Logout()
    {
        // Xóa thông tin đăng nhập khỏi session
        HttpContext.Session.Remove("username");

        // Chuyển hướng người dùng về trang đăng nhập
        return RedirectToAction("Index", "LoginEmployee", new { Area = "Employee" });
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(DEMO_NEXUSPROJECT.Models.Employee employee, IFormFile file)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (file != null) // Check if a file was uploaded
                {
                    var fileName = FileHelper.generateFileName(file.FileName);
                    var path = Path.Combine(webHostEnvironment.WebRootPath, "Images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    employee.Photo = fileName;
                }
                else
                {
                    employee.Photo = "no-image-icon-10.png";
                }
                employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
                employee.Status = true;
                employee.SecurityCode = RandomHelpers.generteSecurityCode();


                if (accountService.create(employee))
                {
                    return RedirectToAction("Index", "LoginEmployee", new { Area = "Employee" });
                }
                else
                {
                    ModelState.AddModelError("", "Error while creating the employee.");
                    return View(employee);
                }
            }
            else
            {
                return View(employee);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error while registering the employee: " + ex.Message);
            return View(employee);
        }
    }

    [Route("forgotpassword")]
    public IActionResult Forgotpassword()
    {
        return View("Forgotpassword");
    }
}


