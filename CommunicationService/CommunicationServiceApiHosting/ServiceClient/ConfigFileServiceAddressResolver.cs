using CommunicationServiceAbstraction;
using Microsoft.Extensions.Configuration;

namespace CommunicationServiceApiHosting.ServiceClient;

public class ConfigFileServiceAddressResolver : IServiceAddressResolver
{
    private readonly IConfiguration _config;

    public ConfigFileServiceAddressResolver(IConfiguration config)
    {
        _config = config;
    }

    public string GetBaseAddress(string serviceId)
    {
        var address = _config[$"services:{serviceId}"];
        return address;
    }

    public string GetBusinessServiceEndpoint(string serviceType)
    {
        var serviceName = serviceType.TrimStart('I');
        var serviceEndpoint = $"api/{serviceName}Host";
        return serviceEndpoint;
    }
}