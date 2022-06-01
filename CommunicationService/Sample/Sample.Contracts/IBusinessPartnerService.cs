using CommunicationServiceAbstraction;

namespace Sample.Contracts
{
    public interface IBusinessPartnerService : IBusinessService
    {
        BPModel GetById(int id);

        Task<BPModel> GetByIdAsync(int id);

        BPModel UpdateModel(int id, string name, BPModel model);

        Task UpdateModelAsync(int id, string name, BPModel model);
    }
}