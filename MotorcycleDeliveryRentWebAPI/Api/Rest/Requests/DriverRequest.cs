using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class DriverRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Cnpj { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Cnh { get; set; }
        public CnhTypeEnum CnhType { get; set; }
        public DriverStatusEnum Status { get; set; }

        internal static DriverModel Convert(DriverRequest request)
        {
            DriverModel model = new DriverModel();
            model.Email = request.Email;
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            model.Name = request.Name;
            model.Cnpj = request.Cnpj;
            model.BirthDate = request.BirthDate;
            model.Cnh = request.Cnh;
            model.Status = DriverStatusEnum.Available;
            return model;
        }
    }
}
