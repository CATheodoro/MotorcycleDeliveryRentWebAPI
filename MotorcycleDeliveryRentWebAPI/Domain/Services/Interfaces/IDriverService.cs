using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IDriverService
    {
        public Task<List<DriverDTO>> GetAllAsync();
        public Task<DriverDTO> GetByIdAsync(string id);
        public Task<List<DriverDTO>> GetByStatusAsync(DriverStatusEnum status);
        public Task<DriverDTO> GetByEmailAsync(string email);
        public Task<DriverDTO> CreateAsync(DriverRequest request);
        public Task<bool> UpdateAsync(string id, DriverUpdateRequest request);
        public Task<bool> UpdatePasswordAsync(string id, PasswordRequest request);
        public Task<bool> UpdateStatus(string id, DriverStatusEnum status);
        public Task<TokenDTO> LoginAsync(LoginRequest request);
        public Task<bool> UploadCnhImageAsync([FromForm] IFormFile image);
        public Task<DriverModel> GetByIdModel(string id);
        public Task<DriverModel> GetByEmailModel(string email);
    }
}
