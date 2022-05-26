using CommunicationServiceAbstraction;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationServiceApiFramework.ServiceClient;

internal class ClientFactoryImpl : IClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ClientFactoryImpl(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T CreateClient<T>() where T : IBusinessService
    {
        var businessService = _serviceProvider.GetRequiredService<T>();
        return businessService;
    }
}