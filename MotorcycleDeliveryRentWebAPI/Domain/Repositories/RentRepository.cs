using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class RentRepository : IRentRepository
    {
        private readonly IMongoCollection<RentModel> _collection;

        public RentRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<RentModel>("Rent");
        }

        public async Task<List<RentModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<RentModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<RentModel>> GetByDriverId(string driverId)
        {
            return await _collection.Find(x => x.DriverId == driverId).ToListAsync();
        }

        public async Task<RentModel> GetByMotorcycleId(string motorcycleId)
        {
            return await _collection.Find(x => x.MotorcycleId == motorcycleId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(RentModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(string id, RentModel model)
        {
            await _collection.ReplaceOneAsync(p => p.Id == id, model);
        }
    }
}
