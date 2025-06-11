    using Microsoft.AspNetCore.Mvc;
    using DEMO_NEXUSPROJECT.Services;

    namespace DEMO_NEXUSPROJECT.Areas.Finance.Controllers;
    [Area("finance")]
    [Route("finance/financeReport")]
    [Route("financeReport")]
    public class FinanceReportController : Controller
    {
        private readonly InvoiceService invoiceService;
        private readonly PaymentService paymentService;

        public FinanceReportController(InvoiceService _invoiceService, PaymentService _paymentService)
        {
            invoiceService = _invoiceService;
            paymentService = _paymentService;
        }

    [HttpGet]
    [Route("Invoices")]
    public IActionResult Invoices(int page = 1)
    {
        int pageSize = 5; // Number of invoices per page

        var pagedInvoices = invoiceService.GetInvoicePaged(page, pageSize);
        var totalInvoices = invoiceService.GetTotalInvoices();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)totalInvoices / pageSize);

        return View(pagedInvoices);
    }

    [Route("Index")]
    public IActionResult Index(int year, int page = 1)
    {
        if (year == 0)
        {
            year = DateTime.Now.Year;
        }

        int pageSize = 10; // Number of items per page
        var invoicesByYear = invoiceService.GetInvoicesByYear(year, page, pageSize);
        var totalInvoices = invoiceService.GetInvoicesByYearCount(year);
        var paymentsByYear = paymentService.GetPaymentsByYear(year);

        ViewBag.Year = year;
        ViewBag.TotalInvoicesAmount = invoicesByYear.Sum(i => i.Amount);
        ViewBag.TotalPaymentsAmount = paymentsByYear.Sum(p => p.Amount);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)totalInvoices / pageSize);

        return View(invoicesByYear);
    }

    [Route("MonthlyReport")]
    public IActionResult MonthlyReport(int year, int month, int page = 1)
    {
        int pageSize = 5;

        var invoices = invoiceService.GetInvoicesByMonth(month, year, page, pageSize);
        var totalInvoices = invoiceService.GetTotalInvoicesByMonth(month, year);
        var payments = paymentService.GetPaymentsByMonth(month, year);

        ViewBag.Year = year;
        ViewBag.Month = month;
        ViewBag.TotalInvoicesAmount = invoices.Sum(i => i.Amount);
        ViewBag.TotalPaymentsAmount = payments.Sum(p => p.Amount);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)totalInvoices / pageSize);

        return View(invoices);
    }

    [Route("QuarterlyReport")]
    public IActionResult QuarterlyReport(int year, int quarter, int page = 1)
    {
        int pageSize = 5;

        var invoices = invoiceService.GetInvoicesByQuarter(quarter, year, page, pageSize);
        var totalInvoices = invoiceService.GetTotalInvoicesByQuarter(quarter, year);
        var payments = paymentService.GetPaymentsByQuarter(quarter, year);

        ViewBag.Year = year;
        ViewBag.Quarter = quarter;
        ViewBag.TotalInvoicesAmount = invoices.Sum(i => i.Amount);
        ViewBag.TotalPaymentsAmount = payments.Sum(p => p.Amount);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = Math.Ceiling((double)totalInvoices / pageSize);

        return View(invoices);
    }

    // GET: /FinancialReport/SelectMonth
    [HttpGet("FinancialReport/SelectMonth")]
        public IActionResult SelectMonthForm()
        {
            return View();
        }

        // POST: /FinancialReport/SelectMonth
        [HttpPost("FinancialReport/SelectMonth")]
        public IActionResult SelectMonth(int year, int month)
        {
            return RedirectToAction("MonthlyReport", new { year = year, month = month });
        }

        // GET: /FinancialReport/SelectQuarter
        [HttpGet("FinancialReport/SelectQuarter")]
        public IActionResult SelectQuarterForm()
        {
            return View();
        }

        // POST: /FinancialReport/SelectQuarter
        [HttpPost("FinancialReport/SelectQuarter")]
        public IActionResult SelectQuarter(int year, int quarter)
        {
            return RedirectToAction("QuarterlyReport", new { year = year, quarter = quarter });
        }
    }
