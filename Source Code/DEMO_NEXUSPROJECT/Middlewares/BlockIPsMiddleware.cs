using DEMO_NEXUSPROJECT.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DEMO_NEXUSPROJECT.Middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class BlockIPsMiddleware
{
    private readonly RequestDelegate _next;
    private IPService pService;
    public BlockIPsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext,IPService ipservice)
    {
        
        var myip = httpContext.Connection.RemoteIpAddress.ToString();
        if(ipservice.isBlock(myip))
        {
            return;
        }
        else
        {
            await _next(httpContext); 
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class BlockIPsMiddlewareExtensions
{
    public static IApplicationBuilder UseBlockIPsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockIPsMiddleware>();
    }
}
