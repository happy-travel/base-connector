using HappyTravel.StdOutLogger.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Collections.Generic;
using HappyTravel.BaseConnector.Api.GrpcServices;
using HappyTravel.BaseConnector.Api.Infrastructure.Environment;
using HappyTravel.EdoContracts.Grpc.Surrogates;

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
        
        EdoContractsSurrogates.Register();

        app.UseRouting()
            .UseHttpMetrics()
            .UseGrpcMetrics()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics().RequireHost($"*:{EnvironmentVariableHelper.GetPort("HTDC_METRICS_PORT")}");
                endpoints.MapHealthChecks("/health").RequireHost($"*:{EnvironmentVariableHelper.GetPort("HTDC_HEALTH_PORT")}");
                endpoints.MapGrpcService<GrpcConnectorService>();
            });

        app.UseHttpContextLogging(options => options.IgnoredPaths = new HashSet<string> { "/health", "/metrics" });
    }
}
