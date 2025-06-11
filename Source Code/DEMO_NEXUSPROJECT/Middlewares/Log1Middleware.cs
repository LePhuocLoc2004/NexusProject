using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DEMO_NEXUSPROJECT.Middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class Log1Middleware
{
    private readonly RequestDelegate _next;

    public Log1Middleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        Debug.WriteLine("datetime: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class dClassExtensions
{
    public static IApplicationBuilder UsedClass(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Log1Middleware>();
    }
}
