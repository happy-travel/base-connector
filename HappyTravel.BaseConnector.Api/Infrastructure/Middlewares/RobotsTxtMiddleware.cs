using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Middlewares;

public class RobotsTxtMiddleware
{
    public RobotsTxtMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/robots.txt"))
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("User-agent: * \nDisallow: /");
        }
        else
        {
            await _next(context);
        }
    }
    
    
    private readonly RequestDelegate _next;
}