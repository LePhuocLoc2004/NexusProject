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
    [Route("employee/invoice")]

    public class InvoiceEmployeeController : Controller
    {
        private readonly AccountEmployeeService accountService;
        private readonly InvoiceEmployeeService invoiceService;
        private IWebHostEnvironment webHostEnvironment;

        public InvoiceEmployeeController(AccountEmployeeService _accountService, InvoiceEmployeeService _invoiceService, IWebHostEnvironment _webHostEnvironment)
        {
            accountService = _accountService;
            invoiceService = _invoiceService;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        [Route("list/{customerId}")]
        public IActionResult List(int customerId)
        {
            var invoice = invoiceService.findById(customerId);
            ViewBag.invoices = invoiceService.findAll();
            return View("List", invoice);
        }


        [HttpGet]
        [Route("edit/{invoiceId}")]
        public IActionResult Edit(int invoiceId)
        {
            var invoice = invoiceService.findByInvoiceId(invoiceId);
            ViewBag.invoices = invoiceService.findAll();
            return View("Edit", invoice);
        }

        [HttpPost]
        [Route("edit/{invoiceId}")]
        public IActionResult Edit(int invoiceId, IFormFile file, Invoice invoice,int customerId)
        {
           
            if (invoiceService.update(invoice))
            {
                TempData["Msg2"] = "Update Success";
                return RedirectToAction("List", new { customerId = customerId });
            }
            else
            {
                TempData["Msg2"] = "Update Failed";
                return RedirectToAction("edit", new { invoiceId = invoiceId });
            }
        }

        [Route("delete/{invoiceId}")]
        public IActionResult Delete(int invoiceId)
        {
            if (invoiceService.delete(invoiceId))
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
