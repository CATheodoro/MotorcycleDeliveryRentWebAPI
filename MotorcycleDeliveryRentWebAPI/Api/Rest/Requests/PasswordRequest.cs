using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class PasswordRequest
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }

        internal static AdminModel ConvertAdmin(AdminModel model, PasswordRequest request)
        {
            model.Password = request.NewPassword;
            return model;
        }

        internal static DriverModel ConvertDriver(DriverModel model, PasswordRequest request)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            return model;
        }
    }
}
