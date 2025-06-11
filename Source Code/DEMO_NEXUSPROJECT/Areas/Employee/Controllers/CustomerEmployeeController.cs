using Castle.Core.Resource;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using DEMO_NEXUSPROJECT.Helpers;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using System.Security.Claims;

namespace DEMO_NEXUSPROJECT.Areas.Employee.Controllers
{
    [Area("employee")]
    [Route("employee/customer")]

    public class CustomerEmployeeController : Controller
    {
        private readonly AccountEmployeeService accountService;
        private readonly CustomerEmployeeService customerService;
        private IWebHostEnvironment webHostEnvironment;

        public CustomerEmployeeController(AccountEmployeeService _accountService, CustomerEmployeeService _customerService, IWebHostEnvironment _webHostEnvironment)
        {
            accountService = _accountService;
            customerService = _customerService;
            webHostEnvironment = _webHostEnvironment;
        }

        [Route("list")]
        [Route("")]
        public IActionResult List()
        {
            ViewBag.customers = customerService.findAll();
            return View("List");
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var customer = new Customer();      
            ViewBag.customers = customerService.findAll();
            return View("Add", customer);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(IFormFile file, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                TempData["Msg1"] = "Add Failed: Invalid data";
                return View("Add", customer);
            }

            try
            {
                customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);
                customer.Status = true;
                customer.SecurityCode = RandomHelpers.generteSecurityCode();

                if (file != null)
                {
                    var fileName = FileHelper.generateFileName(file.FileName);
                    var path = Path.Combine(webHostEnvironment.WebRootPath, "images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    customer.Photo = fileName;
                }

                if (customerService.create(customer))
                {
                    TempData["Msg1"] = "Add Success";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["Msg1"] = "Add Failed";
                    return View("Add", customer); // Trả về view với model để người dùng có thể sửa lỗi
                }
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết để xác định nguyên nhân
                TempData["Msg1"] = "Add Failed: " + ex.Message;
                return View("Add", customer); // Trả về view với model để người dùng có thể sửa lỗi
            }
        }



        [HttpGet]
        [Route("edit/{customerId}")]
        public IActionResult Edit(int customerId)
        {
            var customer = customerService.findById(customerId);
            ViewBag.categories = customerService.findAll();
            return View("Edit", customer);
        }

        [HttpPost]
        [Route("edit/{customerId}")]
        public IActionResult Edit(int customerId, IFormFile file, Customer customer)
        {
            if (file != null)
            {
                var fileName = FileHelper.generateFileName(file.FileName);
                var path = Path.Combine(webHostEnvironment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                customer.Photo = fileName;
            }
            if (customerService.update(customer))
            {
                TempData["Msg2"] = "Update Success";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Msg2"] = "Update Failed";
                return RedirectToAction("edit", new { customerId = customerId });
            }
        }

        [Route("delete/{customerId}")]
        public IActionResult Delete(int customerId)
        {
            if (customerService.delete(customerId))
            {
                TempData["Msg3"] = "Success";
            }
            else
            {
                TempData["Msg3"] = "Failed";
            }

            return RedirectToAction("List");
        }

    }
}
