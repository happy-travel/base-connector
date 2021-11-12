using HappyTravel.BaseConnector.Api.Infrastructure.Environment;
using HappyTravel.ConsulKeyValueClient.ConfigurationProvider.Extensions;
using HappyTravel.StdOutLogger.Extensions;
using HappyTravel.StdOutLogger.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureBaseConnector(this IHostBuilder hostBuilder, string connectorName)
            => hostBuilder
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    var consulHttpAddr = System.Environment.GetEnvironmentVariable("CONSUL_HTTP_ADDR") ?? throw new InvalidOperationException("Consul endpoint is not set");
                    var consulHttpToken = System.Environment.GetEnvironmentVariable("CONSUL_HTTP_TOKEN") ?? throw new InvalidOperationException("Consul HTTP token is not set");
                    config.AddConsulKeyValueClient(consulHttpAddr, "common", consulHttpToken, environment.EnvironmentName, optional: environment.IsLocal());
                    config.AddConsulKeyValueClient(consulHttpAddr, connectorName, consulHttpToken, environment.EnvironmentName, optional: environment.IsLocal());

                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders()
                        .AddConfiguration(hostingContext.Configuration.GetSection("Logging"));

                    var env = hostingContext.HostingEnvironment;
                    if (env.IsLocal())
                        logging.AddConsole();
                    else
                    {
                        logging.AddStdOutLogger(setup =>
                        {
                            setup.IncludeScopes = true;
                            setup.RequestIdHeader = Constants.DefaultRequestIdHeader;
                            setup.UseUtcTimestamp = true;
                        });
                        logging.AddSentry();
                    }
                });
        
    }
}
