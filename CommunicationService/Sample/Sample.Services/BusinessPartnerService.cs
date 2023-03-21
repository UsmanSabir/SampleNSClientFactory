using Sample.Contracts;

namespace Sample.Services
{
    public class BusinessPartnerService : IBusinessPartnerService
    {
        public BPModel GetById(int id)
        {
            return new BPModel()
            {
                Id = id,
                Name = "BPMain",
                DateOfBirth = DateTime.Today.AddYears(-20),
                SomeMetaData = "Dummy data"
            };
        }

        public Task<BPModel> GetByIdAsync(int id)
        {
            var res = GetById(id);
            Thread.Sleep(5000);
            return Task.FromResult(res);
        }

        public BPModel UpdateModel(int id, string name, BPModel model)
        {
            Console.WriteLine($"{id}-{name}-{model.DateOfBirth}");
            model.Name = "Upd";
            return model;
        }

        public async Task UpdateModelAsync(int id, string name, BPModel model)
        {
            Thread.Sleep(5000);
        }
    }
}