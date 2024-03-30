using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface IAdminService
    {
        public List<AdminDTO> GetAll();
        public AdminDTO GetById(string id);
        public AdminDTO GetByEmail(string email);
        public AdminDTO Create(LoginAdminDriverRequest request);
        public string Login(LoginAdminDriverRequest request);
        public bool UpdatePassword(string id, PasswordRequest request);
        public AdminModel GetByIdModel(string id);
        public AdminModel GetByEmailModel(string email);
    }
}
