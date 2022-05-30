using System.Diagnostics;
using System.Net;
using HappyTravel.BaseConnector.Api.Infrastructure.Environment;
using HappyTravel.ConsulKeyValueClient.ConfigurationProvider.Extensions;
using HappyTravel.StdOutLogger.Extensions;
using HappyTravel.StdOutLogger.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureBaseConnectorHost(this WebApplicationBuilder builder, string connectorName)
    {
        #region Configure Configuration

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        #endregion

        #region Configure Consul
        
        var consulHttpAddr = builder.Configuration.GetValue<string>("CONSUL_HTTP_ADDR");
        var consulHttpToken = builder.Configuration.GetValue<string>("CONSUL_HTTP_TOKEN");
        
        builder.Configuration.AddConsulKeyValueClient(url: consulHttpAddr, 
            key: "common", 
            token: consulHttpToken, 
            bucketName: builder.Environment.EnvironmentName, 
            optional: builder.Environment.IsLocal());
        
        builder.Configuration.AddConsulKeyValueClient(url: consulHttpAddr, 
            key: connectorName, 
            token: consulHttpToken, 
            bucketName: builder.Environment.EnvironmentName, 
            optional: builder.Environment.IsLocal());
        
        #endregion

        #region Configure Kestrel
        
        builder.WebHost.UseKestrel(options =>
        {
            options.Listen(IPAddress.Any, builder.Configuration.GetValue<int>("HTDC_WEBAPI_PORT"));
            options.Listen(IPAddress.Any, builder.Configuration.GetValue<int>("HTDC_METRICS_PORT"));
            options.Listen(IPAddress.Any, builder.Configuration.GetValue<int>("HTDC_HEALTH_PORT"));
            options.Listen(IPAddress.Any, builder.Configuration.GetValue<int>("HTDC_GRPC_PORT"), o =>
            {
                o.Protocols = HttpProtocols.Http2;
            });
        });
        
        #endregion

        #region Configure ServiceProvider
        
        builder.WebHost.UseDefaultServiceProvider(s =>
        {
            s.ValidateScopes = true;
            s.ValidateOnBuild = true;
        });
        
        #endregion

        #region Configure Sentry
        
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = builder.Configuration.GetValue<string>("Sentry:Endpoint");
            options.Environment = builder.Environment.EnvironmentName;
            options.IncludeActivityData = true;
            options.BeforeSend = sentryEvent =>
            {
                if (Activity.Current is null)
                    return sentryEvent;

                foreach (var (key, value) in Activity.Current.Baggage)
                    sentryEvent.SetTag(key, value ?? string.Empty);

                sentryEvent.SetTag("TraceId", Activity.Current.TraceId.ToString());
                sentryEvent.SetTag("SpanId", Activity.Current.SpanId.ToString());

                return sentryEvent;
            };
        });
        
        #endregion

        #region Configure Logging

        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
        
        if (builder.Environment.IsLocal())
            builder.Logging.AddConsole();
        else
        {
            builder.Logging.AddStdOutLogger(setup =>
            {
                setup.IncludeScopes = true;
                setup.RequestIdHeader = Constants.DefaultRequestIdHeader;
                setup.UseUtcTimestamp = true;
            });
            builder.Logging.AddSentry();
        }

        #endregion
    }
}