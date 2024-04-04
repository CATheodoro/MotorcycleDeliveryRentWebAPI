using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Requests
{
    public class AdminRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        internal static AdminModel Convert(AdminRequest request)
        {
            AdminModel model = new AdminModel();
            model.Email = request.Email;
            model.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            model.Rule = new List<string> { "Admin" };
            return model;
        }
    }
}
