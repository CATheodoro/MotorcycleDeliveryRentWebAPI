using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class CnhImageRepository : ICnhImageRepository
    {
        private readonly IMongoCollection<CnhImageModel> _collection;

        public CnhImageRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<CnhImageModel>("CnhImage");
        }

        public async Task CreateAsync(CnhImageModel model)
        {
            await _collection.InsertOneAsync(model);
        }
    }
}
