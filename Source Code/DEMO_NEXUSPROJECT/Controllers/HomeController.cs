using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DEMO_NEXUSPROJECT.Controllers;
[Route("home")]
public class HomeController : Controller
{
    private ServicePackageService servicePackageService;
    public HomeController(ServicePackageService _servicePackageService)
    {
        servicePackageService = _servicePackageService;
    }

    [HttpGet]
    [Route("home")]
    [Route("")]
    //[Route("~/")]
    public IActionResult Home()
    {
        //ViewBag.servicepackage = servicePackageService.latestProduct(8);
        //var latestProducts = servicePackageService.latestProduct(3);
        //ViewBag.LaterA = latestProducts;

        //if (latestProducts.Any())
        //{
        //    int lastId = latestProducts.Last().ServicePackageId;
        //    ViewBag.LaterB = servicePackageService.GetNextProducts(lastId, 3);
        //}
        //else
        //{
        //    ViewBag.LaterB = new List<ServicePackage>();
        //}
        return View("home");
    }
}
