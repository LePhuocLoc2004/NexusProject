using DEMO_NEXUSPROJECT.Models;
using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NEXUSPROJECT.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

var connectionStrings = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(connectionStrings);
});

builder.Services.AddScoped<AccountAdminService, AccountAdminServiceImpl>();
builder.Services.AddScoped<ServicePackageService, ServicePackageServiceImpl>();
builder.Services.AddScoped<RetailstoreService, RetailstoreServiceImpl>();
builder.Services.AddScoped<CustomerService, CustomerServiceImpl>();
builder.Services.AddScoped<MailServices, MailServicesImpl>();
builder.Services.AddScoped<OrdersService, OrdersServiceImpl>();
builder.Services.AddScoped<InvoiceService, InvoiceServiceImpl>();
builder.Services.AddScoped<PaymentService, PaymentServiceImpl>();
builder.Services.AddScoped<ConnectionsServices, ConnectionsServicesImpl>();
builder.Services.AddScoped<FinanceService, FinanceServiceImpl>();
builder.Services.AddScoped<ConnectionRequestService, ConnectionRequestServiceimpl>();
builder.Services.AddScoped<ConnectionPackageService, ConnectionPackageServiceImpl>();
builder.Services.AddScoped<TransactionLogService, TransactionLogServiceImpl>();
builder.Services.AddScoped<OrderService, OrderServiceImpl>();
builder.Services.AddScoped<FeedbackService, FeedbackServiceImpl>();
builder.Services.AddScoped<AccountEmployeeService, AccountEmployeeServiceImpl>();
builder.Services.AddScoped<CustomerEmployeeService, CustomerEmployeeServiceImpl>();
builder.Services.AddScoped<RoleEmployeeService, RoleEmployeeServiceImpl>();
builder.Services.AddScoped<ProductEmployeeService, ProductEmployeeServiceImpl>();
builder.Services.AddScoped<InvoiceEmployeeService, InvoiceEmployeeServiceImpl>();
builder.Services.AddScoped<OrderEmployeeService, OrderEmployeeServiceImpl>();
builder.Services.AddScoped<ContactService, ContactServiceImpl>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login";
        options.AccessDeniedPath = "/account/accessdenied";
    });

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

var cultures = new CultureInfo[]
{
    new CultureInfo("en-US"),
    new CultureInfo("vi-VN"),
    new CultureInfo("fr-FR"),
    new CultureInfo("ja-JP")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultures[0]),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});

app.MapControllerRoute(
    name: "myareas",
    pattern: "{area:exists}/{controller}/{action}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}"
);

app.Run();
