using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly IMongoCollection<DriverModel> _collection;

        public DriverRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<DriverModel>("Driver");
        }

        public async Task<List<DriverModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<DriverModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DriverModel> GetByEmail(string email)
        {
            return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<DriverModel>> GetByStatus(DriverStatusEnum status)
        {
            return await _collection.Find(x => x.Status == status).ToListAsync();
        }

        public async Task<DriverModel> GetByCnh(string cnh)
        {
            return await _collection.Find(x => x.Cnh == cnh).FirstOrDefaultAsync();
        }

        public async Task<DriverModel> GetByCnpj(string cnpj)
        {
            return await _collection.Find(x => x.Cnpj == cnpj).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(DriverModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(string id, DriverModel model)
        {
            await _collection.ReplaceOneAsync(p => p.Id == id, model);
        }
    }
}
