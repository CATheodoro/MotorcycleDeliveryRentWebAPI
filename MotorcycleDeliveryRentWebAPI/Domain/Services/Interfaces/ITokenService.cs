namespace MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(string id, string email, List<string> rules);
    }
}
