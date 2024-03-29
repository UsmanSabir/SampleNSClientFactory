﻿using Sample.Contracts;
using System.Diagnostics;

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
            return Task.FromResult(res);
        }

        public void NotifyEventId(int id)
        {
            Debug.WriteLine($"Event Id {id}");
        }

        public BPModel UpdateModel(int id, string name, BPModel model)
        {
            Console.WriteLine($"{id}-{name}-{model.DateOfBirth}");
            model.Name = "Upd";
            return model;
        }

        public async Task UpdateModelAsync(int id, string name, BPModel model)
        {
            
        }

    }
}