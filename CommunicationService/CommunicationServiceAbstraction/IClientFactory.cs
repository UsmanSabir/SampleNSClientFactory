namespace CommunicationServiceAbstraction;

public interface IClientFactory
{
    T CreateClient<T>() where T : IBusinessService;
}