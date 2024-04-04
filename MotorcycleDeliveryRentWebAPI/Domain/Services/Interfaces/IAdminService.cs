using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<AdminDTO>> GetAllAsync();
        public Task<AdminDTO> GetByIdAsync(string id);
        public Task<AdminDTO> GetByEmailAsync(string email);
        public Task<AdminDTO> CreateAsync(AdminRequest request);
        public Task<TokenDTO> Login(LoginRequest request);
        public Task<bool> UpdatePassword(string id, PasswordRequest request);
        public Task<AdminModel> GetByIdModel(string id);
        public Task<AdminModel> GetByEmailModel(string email);
    }
}
