using CommunicationServiceAbstraction;

namespace Sample.Contracts
{
    public interface IBusinessPartnerService : IBusinessService
    {
        BPModel GetById(int id);
    }
}