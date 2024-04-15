using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {

        private readonly IMongoCollection<MotorcycleModel> _collection;

        public MotorcycleRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<MotorcycleModel>("Motorcycle");
        }


        public async Task<List<MotorcycleModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<MotorcycleModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<MotorcycleModel> GetByPlate(string plate)
        {
            return await _collection.Find(x => x.Plate == plate).FirstOrDefaultAsync();
        }

        public async Task<MotorcycleModel> GetFirstAvailable()
        {
            return await _collection.Find(x => x.Status == MotorcycleStatusEnum.Available).FirstOrDefaultAsync();
        }

        public async Task Create(MotorcycleModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task Update(string id, MotorcycleModel model)
        {
            await _collection.ReplaceOneAsync(p => p.Id == id, model);
        }

        public async Task Delete(string id)
        {
            await _collection.FindOneAndDeleteAsync(p => p.Id == id);
        }
    }
}
