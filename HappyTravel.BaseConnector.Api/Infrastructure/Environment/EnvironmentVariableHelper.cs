using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Environment;

public static class EnvironmentVariableHelper
{
    public static string Get(string key, IConfiguration configuration)
    {
        var environmentVariable = configuration[key] ?? throw new Exception($"Couldn't obtain the value for '{key}' configuration key.");
        return System.Environment
            .GetEnvironmentVariable(environmentVariable) ?? throw new Exception($"Couldn't obtain the value for '{environmentVariable}' in configuration"); ;
    }


    public static bool IsLocal(this IHostEnvironment hostingEnvironment)
        => hostingEnvironment.IsEnvironment(LocalEnvironment);
    
    
    public static int GetPort(string key)
    {
        var value = System.Environment.GetEnvironmentVariable(key);
        if (!int.TryParse(value, out var port))
            throw new Exception($"{key} is not set");

        return port;
    }


    private const string LocalEnvironment = "Local";
}
