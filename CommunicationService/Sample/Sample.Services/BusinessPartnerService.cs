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

        public BPModel UpdateModel(int id, string name, BPModel model)
        {
            Console.WriteLine($"{id}-{name}-{model.DateOfBirth}");
            model.Name = "Upd";
            return model;
        }
    }
}