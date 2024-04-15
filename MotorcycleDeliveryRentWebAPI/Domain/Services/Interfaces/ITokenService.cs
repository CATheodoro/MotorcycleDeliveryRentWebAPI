using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface ITokenService
    {
        public TokenDTO CreateToken(string id, string email, List<string> rules);
    }
}
