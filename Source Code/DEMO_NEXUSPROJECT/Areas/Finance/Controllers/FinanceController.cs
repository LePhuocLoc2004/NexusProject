using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DEMO_NEXUSPROJECT.Services;
using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Areas.Finance.Controllers;
[Area("finance")]
[Route("finance")]
public class FinanceController : Controller
{
    private readonly InvoiceService invoiceService;
    private readonly FinanceService financeService;
    private readonly AccountAdminService accountadmintService;
  

    public FinanceController(InvoiceService _invoiceService, FinanceService _financeService, AccountAdminService _accountAdminService  )
    {
        invoiceService = _invoiceService;
        financeService = _financeService;
        accountadmintService = _accountAdminService;
    }

   
  
    [Route("financeDashboard")]
    public IActionResult FinanceDashboard()
    {
        var finances = financeService.GetAllFinances();
        double totalAmountSum = financeService.GetTotalAmountSum();
        int totalCustomers = financeService.GetTotalCustomers();
        double totalAmountPaid = financeService.GetTotalAmountPaid();
        double totalRemainingAmount = financeService.GetTotalRemainingAmount();

        // Pass data to ViewBag or ViewData
        ViewBag.TotalAmountSum = totalAmountSum;
        ViewBag.TotalCustomers = totalCustomers;
        ViewBag.TotalAmountPaid = totalAmountPaid;
        ViewBag.TotalRemainingAmount = totalRemainingAmount;
        return View(finances);
    }

    [HttpGet]
    [Route("login")]
    [Route("")]
    public IActionResult Login()
    {
        return View("Login");
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(string email, string password)
    {
        if (accountadmintService.login(email, password, "Finance"))
        {
            return RedirectToAction("financeDashboard", "finance", new { Area = "finance" });
        }
        else
        {
            TempData["Msg"] = "InvaLid";
            return RedirectToAction("login", "finance", new { Area = "finance" });
        }
    }
    //[Route("searchByKeyword")]
    //public IActionResult SearchByKeyword(string keyword)
    //{
    //    var finances = financeService.findByKeyword(keyword);
    //    return View("FinanceDashboard", finances);
    //}


}


