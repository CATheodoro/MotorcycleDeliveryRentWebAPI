using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {

        private readonly IMongoCollection<DeliveryModel> _collection;

        public DeliveryRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<DeliveryModel>("Delivery");
        }


        public async Task<List<DeliveryModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<DeliveryModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(DeliveryModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(string id, DeliveryModel model)
        {
            await _collection.ReplaceOneAsync(p => p.Id == id, model);
        }
    }
}
