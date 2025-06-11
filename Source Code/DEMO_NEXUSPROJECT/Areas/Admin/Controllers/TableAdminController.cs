using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace DEMO_NEXUSPROJECT.Areas.Admin.Controllers;
[Area("admin")]
[Route("admin/table")]
public class TableAdminController : Controller
{
    private ServicePackageService servicePackageService;
    public TableAdminController(ServicePackageService _servicePackageService)
    {
        servicePackageService = _servicePackageService;
    }
    [HttpGet]
    [Route("addservicepackage")]
    [Route("index")]
    [Route("edit")]
    [Route("")]
    public IActionResult Index(int page = 1, int? servicePackageId = null)
    {
        if (servicePackageId.HasValue)
        {
            var service = servicePackageService.findById(servicePackageId.Value);
            ViewBag.SelectedServicePackage = service;
        }
        else
        {
            ViewBag.SelectedServicePackage = new ServicePackage();
        }

        ViewBag.ServicePackages = servicePackageService.findAll();
        int pageSize = 5; // Số lượng sản phẩm trên mỗi trang

        var servicePackages = servicePackageService.GetServicePackagesPaged(page, pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)servicePackageService.GetTotalServicePackages() / pageSize);

        return View(servicePackages);
    }

    [HttpPost]
    [Route("addservicepackage")]
    public IActionResult Addservicepackage(ServicePackage servicePackage)
    {
        if (servicePackage.ServicePackageId > 0)
        {
            if (servicePackageService.Update(servicePackage))
            {
                TempData["Msg"] = "Success";
            }
            else
            {
                TempData["Msg"] = "Failed";
            }
        }
        else
        {
            if (servicePackageService.created(servicePackage))
            {
                TempData["Msg"] = "Success";
            }
            else
            {
                TempData["Msg"] = "Failed";
            }
        }
        return RedirectToAction("Index", "table", new { Area = "admin" });
    }
}
