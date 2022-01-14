# BaseConnector
Base connector - a set of controllers, service interfaces and various infrastructure elements for connectors
Version 0.9.0 supported .NET 5. From version 1.0.0 BaseConnector used .NET 6.

# How to use the BaseConnector library

BaseConnector includes a set of controllers, service interfaces for the connector. Therefore, you do not need to duplicate them in the connector. In the connector, you write a client to connect to the supplier, interact with the connector's internal database, implement services according to the interfaces from the BaseConnector.

## Startup.cs

public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
{
    Configuration = configuration;
    HostEnvironment = hostEnvironment;
}

public void ConfigureServices(IServiceCollection services)
{
    using var vaultClient = new VaultClient.VaultClient(new VaultOptions
    {
        BaseUrl = new Uri(EnvironmentVariableHelper.Get("Vault:Endpoint", Configuration)),
        Engine = Configuration["Vault:Engine"],
        Role = Configuration["Vault:Role"]
    });
    vaultClient.Login(EnvironmentVariableHelper.Get("Vault:Token", Configuration)).GetAwaiter().GetResult();

    services.AddBaseConnectorServices(Configuration, HostEnvironment, vaultClient, ConnectorName);
    
    ***
}

private const string ConnectorName = "XXX Connector";

public IConfiguration Configuration { get; }
public IHostEnvironment HostEnvironment { get; }
