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
    [Route("employee/order")]

    public class OrderEmployeeController : Controller
    {
        private readonly AccountEmployeeService accountService;
        private readonly OrderEmployeeService orderService;
        private IWebHostEnvironment webHostEnvironment;

        public OrderEmployeeController(AccountEmployeeService _accountService, OrderEmployeeService _orderService, IWebHostEnvironment _webHostEnvironment)
        {
            accountService = _accountService;
            orderService = _orderService;
            webHostEnvironment = _webHostEnvironment;
        }

        [Route("list")]
        [Route("")]
        public IActionResult List()
        {
            ViewBag.orders = orderService.findAll();
            return View("List");
        }


        [HttpGet]
        [Route("edit/{orderId}")]
        public IActionResult Edit(int orderId)
        {
            var order = orderService.findById(orderId);
            ViewBag.orders = orderService.findAll();
            return View("Edit", order);
        }

        [HttpPost]
        [Route("edit/{OrderId}")]
        public IActionResult Edit(int orderId, IFormFile file, Order order)
        {

            if (orderService.update(order))
            {
                TempData["Msg2"] = "Update Success";
                return RedirectToAction("List", new { orderId = orderId });
            }
            else
            {
                TempData["Msg2"] = "Update Failed";
                return RedirectToAction("edit", new { orderId = orderId });
            }
        }

        [Route("delete/{orderId}")]
        public IActionResult Delete(int orderId)
        {
            if (orderService.delete(orderId))
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
