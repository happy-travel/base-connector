# BaseConnector
Base connector - a set of controllers, service interfaces and various infrastructure elements for connectors
Version 0.9.0 supported .NET 5. From version 1.0.0 BaseConnector used .NET 6.

# How to use the BaseConnector library

BaseConnector includes a set of controllers, service interfaces for the connector. Therefore, you do not need to duplicate them in the connector. In the connector, you write a client to connect to the supplier, interact with the connector's internal database, implement services according to the interfaces from the BaseConnector.

## Program.cs
```c#
using HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBaseConnectorHost("connector-name");
builder.ConfigureServices("connector-name");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureBaseConnector(builder.Configuration);
app.Run();
```

## Startup.cs
Not needed

## ConfigureServicesExtension
```c#
public static void ConfigureServices(this WebApplicationBuilder builder, string connectorName)
{
    using var vaultClient = new VaultClient.VaultClient(new VaultOptions
    {
        BaseUrl = new Uri(builder.Configuration[builder.Configuration["Vault:Endpoint"]]),
        Engine = builder.Configuration["Vault:Engine"],
        Role = builder.Configuration["Vault:Role"]
    });
    vaultClient.Login(builder.Configuration["Vault:Token"]).GetAwaiter().GetResult();

    services.AddBaseConnectorServices(Configuration, HostEnvironment, vaultClient, ConnectorName);
    
    ***
}
```

## Before installing remove the following packages

HappyTravel.Telemetry
HappyTravel.EdoContracts
HappyTravel.VaultClient
CSharpFunctionalExtensions

These packages will be implicitely reinstalled by BaseConnector
