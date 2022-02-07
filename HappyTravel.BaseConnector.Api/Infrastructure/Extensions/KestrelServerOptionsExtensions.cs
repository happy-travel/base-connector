using System.Net;
using HappyTravel.BaseConnector.Api.Infrastructure.Environment;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class KestrelServerOptionsExtensions
{
    public static KestrelServerOptions ConfigureBaseConnector(this KestrelServerOptions options)
    {
        options.Listen(IPAddress.Any, EnvironmentVariableHelper.GetPort("HTDC_WEBAPI_PORT"));
        options.Listen(IPAddress.Any, EnvironmentVariableHelper.GetPort("HTDC_METRICS_PORT"));
        options.Listen(IPAddress.Any, EnvironmentVariableHelper.GetPort("HTDC_HEALTH_PORT"));
        options.Listen(IPAddress.Any, EnvironmentVariableHelper.GetPort("HTDC_GRPC_PORT"), o =>
        {
            o.Protocols = HttpProtocols.Http2;
        });

        return options;
    }
}