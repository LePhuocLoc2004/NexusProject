using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;

namespace DEMO_NEXUSPROJECT.Areas.Finance.Controllers;
[Area("finance")]
[Route("finance/customer")]
[Route("customer")]
public class CustomerController : Controller
{
    private CustomerService customerService;
    private MailServices mailService;
    private IConfiguration configuration;
    private FeedbackService feedbackService;
    

    public CustomerController(CustomerService _customerService, MailServices _mailService , IConfiguration _configuration , FeedbackService _feedbackService)
    {
        customerService = _customerService;
        this.mailService = _mailService;
        this.configuration = _configuration;
        this.feedbackService = _feedbackService;
    }

    [HttpGet]
    [Route("ListCustomer")]
    public IActionResult ListCustomer()
    {
        var customers = customerService.GetAllCustomers();
        return View(customers);
    }

    [HttpGet]
    public ActionResult ShowOutstandingFees(int customerId)
    {
        // Gọi service để lấy danh sách các khoản phí còn nợ của khách hàng
        var outstandingFees = customerService.GetOutstandingFees(customerId);

        // Truyền danh sách này cho view để hiển thị
        return View(outstandingFees);
    }



    [HttpPost]
    public IActionResult NotifyOutstandingFees(int customerId) 
    {
        try
        {
            // Retrieve customer and outstanding fees
            var customer = customerService.GetCustomerById(customerId);
            var outstandingFees = customerService.GetOutstandingFees(customerId);
            double totalAmount = outstandingFees.Sum(f => f.Amount);

            if (customer != null && outstandingFees != null && outstandingFees.Count > 0)
            {
                var body = $"Hello {customer.FullName},<br/><br/>" +
                           $"Phone Number: {customer.PhoneNumber}<br/>" +
                           $"Address: {customer.Address}<br/><br/>" +
                           $"Below is the list of your outstanding fees:<br/><br/>";
                foreach (var fee in outstandingFees)
                {
                    body += $"Invoice ID: {fee.InvoiceId}<br/>" +
                            $"Outstanding Amount: {fee.Amount.ToString("C")}<br/>" +
                            $"Due Date: {fee.IssueDate.ToShortDateString()}<br/><br/>";
                }
                body += $"Total Outstanding Amount: {totalAmount.ToString("C")}<br/>";
                body += "Please make the payment to avoid further penalty fees.<br/><br/>Best regards,<br/>Nexus Project Team";

                // Send email notification
                if (mailService.Send("12082004huy@gmail.com", customer.Email, "Outstanding Fees Notification", body))
                {
                    TempData["Msg"] = "Email notification sent successfully.";
                }
                else
                {
                    TempData["Msg"] = "Failed to send email notification.";
                }
            }
            else
            {
                TempData["Msg"] = "Customer information not found or no outstanding fees.";
            }
        }
        catch (Exception ex)
        {
            TempData["Msg"] = $"An error occurred while sending email: {ex.Message}";
        }

        return RedirectToAction("ListCustomer");
    }

    private decimal CalculatePenaltyFee(decimal amount, DateTime issueDate)
    {
        var overdueDays = (DateTime.Now - issueDate).Days - 30;
        if (overdueDays > 0)
        {
            return amount * 0.01m * overdueDays;
        }
        return 0;
    }

    [HttpGet]
    [Route("FeedBack")]
    public IActionResult FeedBack()
    {
        var feedbackList = feedbackService.GetAllFeedback();
        return View(feedbackList);
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult DeleteFeedback(int id)
    //{
    //    var feedback = feedbackService.GetFeedbackById(id);
    //    if (feedback == null)
    //    {
    //        return NotFound();
    //    }
    //    feedbackService.DeleteFeedback(id);
    //    return RedirectToAction(nameof(Index));
    //}

    [Route("searchByKeyword")]
    public IActionResult SearchByKeyword(string keyword)
    {
        var customers = customerService.findByKeyword(keyword);
        return View("ListCustomer", customers);
    }

    [Route("searchByKeywords")]
    public IActionResult SearchByKeywords(string keyword)
    {
        var feedback = customerService.findByKeywords(keyword);
        return View("Feedback", feedback);
    }





}
