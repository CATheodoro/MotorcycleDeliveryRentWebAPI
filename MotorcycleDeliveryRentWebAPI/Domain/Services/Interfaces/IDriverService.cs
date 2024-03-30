using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IDriverService
    {
        public List<DriverDTO> GetAll();
        public DriverDTO GetById(string id);
        public List<DriverDTO> GetByStatus(DriverStatusEnum status);
        public DriverDTO GetByEmail(string email);
        public DriverDTO Create(DriverRequest request);
        public bool Update(string id, DriverUpdateRequest request);
        public bool UpdatePassword(string id, PasswordRequest request);
        public bool UpdateStatus(string id, DriverStatusEnum status);
        public string Login(LoginAdminDriverRequest request);
        public DriverModel GetByIdModel(string id);
        public DriverModel GetByEmailModel(string email);
        public bool UploadCnhImage([FromForm] IFormFile image);
    }
}
