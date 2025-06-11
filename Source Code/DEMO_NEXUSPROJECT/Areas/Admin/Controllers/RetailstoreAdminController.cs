using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_NEXUSPROJECT.Areas.Admin.Controllers;
[Area("admin")]
[Route("admin/retailstore")]
public class RetailstoreAdminController : Controller
{
    private RetailstoreService retailstoreService;
    public RetailstoreAdminController(RetailstoreService _retailstoreService)
    {
        retailstoreService = _retailstoreService;
    }

    [HttpGet]
    [Route("index")]
    public IActionResult Index()
    {
        ViewBag.retailStore = retailstoreService.findAll();
        return View();
    }

    [HttpGet]
    [Route("add")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Add(RetailStore retailStore)
    {
        if (retailstoreService.create(retailStore))
        {
            TempData["Msg"] = "Succcess";
            return RedirectToAction("index", "Retailstore", new { Area = "admin" });
        }
        else
        {
            TempData["Msg"] = "Failed";
            return RedirectToAction("add");
        }
    }
}
