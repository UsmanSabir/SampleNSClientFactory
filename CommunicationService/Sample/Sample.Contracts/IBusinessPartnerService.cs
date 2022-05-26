using CommunicationServiceAbstraction;

namespace Sample.Contracts
{
    public interface IBusinessPartnerService : IBusinessService
    {
        BPModel GetById(int id);

        BPModel UpdateModel(int id, string name, BPModel model);
    }
}