using HappyTravel.StdOutLogger.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Collections.Generic;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureBaseConnector(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseHealthChecks("/health");
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/robots.txt"))
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("User-agent: * \nDisallow: /");
            }
            else
            {
                await next();
            }
        });

        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseRouting()
            .UseHttpMetrics()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

        app.UseHttpContextLogging(options => options.IgnoredPaths = new HashSet<string> { "/health", "/metrics" });
    }
}
