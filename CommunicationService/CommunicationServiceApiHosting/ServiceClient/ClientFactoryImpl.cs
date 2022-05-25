using CommunicationServiceAbstraction;

namespace CommunicationServiceApiHosting.ServiceClient;

internal class ClientFactoryImpl:IClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ClientFactoryImpl(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T CreateClient<T>() where T : IBusinessService
    {
        throw new NotImplementedException();
    }
}