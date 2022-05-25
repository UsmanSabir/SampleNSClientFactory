namespace CommunicationServiceAbstraction;

public interface IServiceAddressResolver
{
    string GetBaseAddress(string serviceId);
    string GetBusinessServiceEndpoint(string serviceType);
}