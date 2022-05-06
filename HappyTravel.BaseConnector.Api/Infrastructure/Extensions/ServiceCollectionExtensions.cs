using System;
using CacheFlow.Json.Extensions;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Conventions;
using HappyTravel.BaseConnector.Api.Infrastructure.Environment;
using HappyTravel.ErrorHandling.Extensions;
using HappyTravel.Telemetry.Extensions;
using HappyTravel.VaultClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProtoBuf.Grpc.Server;
using System.Collections.Generic;
using System.Globalization;
using HappyTravel.BaseConnector.Api.Infrastructure.Options;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBaseConnectorServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment hostEnvironment, VaultClient.VaultClient vaultClient, string connectorName)
    {
        var serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None
        };
        JsonConvert.DefaultSettings = () => serializationSettings;

        services.AddResponseCompression()
            .AddCors()
            .AddLocalization()
            .AddMemoryCache()
            .AddMemoryFlow()
            .AddStackExchangeRedisCache(options => { options.Configuration = EnvironmentVariableHelper.Get("Redis:Endpoint", configuration); })
            .AddDoubleFlow()
            .AddCacheFlowJsonSerialization()
            .AddWebEncoders()
            .AddTracing(configuration, options =>
            {
                options.ServiceName = $"{hostEnvironment.ApplicationName}-{hostEnvironment.EnvironmentName}";
                options.JaegerHost = hostEnvironment.IsLocal()
                    ? configuration.GetValue<string>("Jaeger:AgentHost")
                    : configuration.GetValue<string>(configuration.GetValue<string>("Jaeger:AgentHost"));
                options.JaegerPort = hostEnvironment.IsLocal()
                    ? configuration.GetValue<int>("Jaeger:AgentPort")
                    : configuration.GetValue<int>(configuration.GetValue<string>("Jaeger:AgentPort"));
                options.RedisEndpoint = configuration.GetValue<string>(configuration.GetValue<string>("Redis:Endpoint"));
            });

        services.ConfigureAuthentication(configuration, vaultClient);

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddMvcCore(options =>
        {
            options.Conventions.Add(new AuthorizeControllerModelConvention());
        })
            .AddAuthorization()
            .AddControllersAsServices()
            .AddMvcOptions(m => m.EnableEndpointRouting = true)
            .AddFormatterMappings()
            .AddNewtonsoftJson()
            .AddApiExplorer()
            .AddCacheTagHelper()
            .AddDataAnnotations();
        services.AddHttpContextAccessor();
        services.AddOptions()
            .Configure<RequestLocalizationOptions>(o =>
            {
                o.DefaultRequestCulture = new RequestCulture("en");
                o.SupportedCultures = new[]
                {
                        new CultureInfo("en")
                };

                o.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = o });
            })
            .Configure<FlowOptions>(options =>
            {
                options.DataLoggingLevel = DataLogLevel.Normal;
                options.SuppressCacheExceptions = false;
                options.CacheKeyDelimiter = "::";
                options.CacheKeyPrefix = $"HappyTravel::{connectorName.Replace(" ", string.Empty)}";
            });
        services.AddProblemDetailsErrorHandling();
        services.AddCodeFirstGrpc();

        return services;
    }


    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration, IVaultClient vaultClient)
    {
        var authorityOptions = configuration.GetSection("Authority").Get<AuthorityOptions>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = authorityOptions.AuthorityUrl;
                options.RequireHttpsMetadata = true;
                options.Audience = authorityOptions.Audience;
                options.AutomaticRefreshInterval = authorityOptions.AutomaticRefreshInterval;
            });

        return services;
    }
}
