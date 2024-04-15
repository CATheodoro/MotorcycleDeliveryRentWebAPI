using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly IMongoCollection<PlanModel> _collection;

        public PlanRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<PlanModel>("Plan");
        }

        public async Task<List<PlanModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<PlanModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(PlanModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(string id, PlanModel model)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, model);
        }
    }
}
