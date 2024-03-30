using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class LoginAdminDriverRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        internal static AdminModel ConvertAdmin(LoginAdminDriverRequest request)
        {
            AdminModel model = new AdminModel();
            model.Email = request.Email;
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            return model;
        }

        internal static DriverModel ConvertDriver(LoginAdminDriverRequest request)
        {
            DriverModel model = new DriverModel();
            model.Email = request.Email;
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            return model;
        }
    }
}
