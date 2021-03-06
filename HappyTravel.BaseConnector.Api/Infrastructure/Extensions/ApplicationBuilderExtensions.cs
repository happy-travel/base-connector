using Microsoft.AspNetCore.Builder;
using Prometheus;
using HappyTravel.BaseConnector.Api.GrpcServices;
using HappyTravel.BaseConnector.Api.Infrastructure.Middlewares;
using HappyTravel.EdoContracts.Grpc.Surrogates;
using Microsoft.Extensions.Configuration;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureBaseConnector(this IApplicationBuilder app, IConfiguration configuration)
    {
        EdoContractsSurrogates.Register();

        app.UseRobotsTxt()
            .UseRouting()
            .UseHttpMetrics()
            .UseGrpcMetrics()
            .UseAuthentication()
            .UseAuthorization()
            .UseResponseCompression()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics().RequireHost($"*:{configuration.GetValue<int>("HTDC_METRICS_PORT")}");
                endpoints.MapHealthChecks("/health").RequireHost($"*:{configuration.GetValue<int>("HTDC_HEALTH_PORT")}");
                endpoints.MapGrpcService<ConnectorGrpcService>();
            });
    }


    private static IApplicationBuilder UseRobotsTxt(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RobotsTxtMiddleware>();
    }
}
