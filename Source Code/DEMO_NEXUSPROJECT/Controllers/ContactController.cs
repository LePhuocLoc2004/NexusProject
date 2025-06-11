using DEMO_NEXUSPROJECT.Models;

using Microsoft.AspNetCore.Mvc;
using NEXUSPROJECT.Services;

namespace DEMO_NEXUSPROJECT.Controllers;
[Route("contact")]
public class ContactController : Controller
{
    private readonly ContactService contactService;
    public ContactController(ContactService _contactService)
    {
        contactService = _contactService;
    }

    [HttpGet]
    [Route("contact")]
    public IActionResult Contact()
    {
        return View();
    }

    // Phương thức POST để xử lý form liên hệ
    [HttpPost]
    [Route("contact")]
    public IActionResult Contact(ContactMessage contactMessage)
    {
        if (contactService.SaveContactMessage(contactMessage))
        {

            TempData["Msg"] = "Gui Contact Thanh Cong";
            return RedirectToAction("Contact");

        }
        else
        {
            TempData["Msg"] = "Gui Contact That bai.";
            return RedirectToAction("Contact");
        }
    }
}
