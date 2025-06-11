using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;

namespace DEMO_NEXUSPROJECT.Areas.Finance.Controllers;
[Area("finance")]
[Route("finance/invoice")]
[Route("invoice")]
public class InvoiceController : Controller
{
    private readonly InvoiceService invoiceService;
    private readonly FinanceService financeService;
    
    private DatabaseContext db;

    public InvoiceController(InvoiceService _invoiceService, FinanceService _financeService,  DatabaseContext _db)
    {
        db = _db;
        invoiceService = _invoiceService;
        financeService = _financeService;
      
    }



    [HttpGet]
    [Route("Invoices")]
    public IActionResult Invoices(int page = 1)
    {
        int pageSize = 10; // Số lượng hóa đơn trên mỗi trang

        // Lấy danh sách hóa đơn có phân trang
        var pagedInvoices = invoiceService.GetInvoicePaged(page, pageSize);

        // Tổng số hóa đơn
        var totalInvoices = invoiceService.GetTotalInvoices();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)totalInvoices / pageSize);

        return View(pagedInvoices);
    }





    [HttpGet]
    [Route("invoice")]
    public IActionResult GetInvoice(int invoiceId)
    {
        var invoice = invoiceService.GetInvoice(invoiceId);
        return View("InvoiceDetails", invoice);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var invoice = invoiceService.GetInvoice(id);
        if (invoice == null)
        {
            return NotFound();
        }
        return View(invoice);
    }

    [HttpGet]
    [Route("createInvoices")]
    public IActionResult CreateInvoices()
    {
        return View();
    }


    

    [Route("searchByKeyword")]
    public IActionResult SearchByKeyword(string keyword)
    {
        var invoices = invoiceService.findByKeyword(keyword);
        return View("Invoices" , invoices);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var result = invoiceService.DeleteInvoice(id);
        if (result)
        {
            TempData["Msg"] = "Invoice deleted successfully.";
        }
        else
        {
            TempData["Msg"] = "Failed to delete invoice.";
        }

        return RedirectToAction("Invoices");
    }

}









