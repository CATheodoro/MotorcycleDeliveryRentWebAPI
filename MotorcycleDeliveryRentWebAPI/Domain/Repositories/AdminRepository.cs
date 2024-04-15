using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<AdminModel> _collection;

        public AdminRepository(IMongoDBSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<AdminModel>("Admin");
        }

        public async Task<List<AdminModel>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<AdminModel> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AdminModel> GetByEmail(string email)
        {
            return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(AdminModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(string id, AdminModel model)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, model);
        }
    }
}
