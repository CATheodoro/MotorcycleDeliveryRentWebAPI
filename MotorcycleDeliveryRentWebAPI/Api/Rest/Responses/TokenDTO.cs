namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Responses
{
    public class TokenDTO
    {
        public string TokenType { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string AccessToken { get; set; }

        public TokenDTO(string email, List<string> roles, string accessToken)
        {
            TokenType = "Bearer";
            Email = email;
            Roles = roles;
            AccessToken = accessToken;
        }
    }
}
