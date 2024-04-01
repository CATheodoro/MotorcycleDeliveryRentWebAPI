using Microsoft.IdentityModel.Tokens;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminModel> _logger;

        public AdminService(IAdminRepository adminRepository, IConfiguration configuration, ILogger<AdminModel> logger)
        {
            _repository = adminRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<AdminDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await AdminDTO.Convert(model);
        }

        public async Task<AdminDTO> GetByIdAsync(string id)
        {
            AdminModel model = await _repository.GetById(id);
            if (model != null)
                return await AdminDTO.Convert(model);

            _logger.LogDebug($"Admin with Id = {id} not found");
            return null;
        }

        public async Task<AdminDTO> GetByEmailAsync(string email)
        {
            AdminModel model = await _repository.GetByEmail(email);

            if (model != null)
                return await AdminDTO.Convert(model);

            _logger.LogError($"Admin with E-mail = {email} not found");
            return null;
        }

        public async Task<AdminDTO> CreateAsync(LoginAdminDriverRequest request)
        {
            ValidatorAdminDriver.Password(request.Password);
            ValidatorAdminDriver.Email(request.Email);
            var email = await GetByEmailAsync(request.Email);

            if (email != null)
            {
                _logger.LogError($"The E-mail must be unique, E-mail = {request.Email}");
                throw new Exception($"The E-mail must be unique, E-mail = {request.Email}");
            }

            AdminModel model = LoginAdminDriverRequest.ConvertAdmin(request);
            await _repository.CreateAsync(model);
            return await AdminDTO.Convert(model);
        }

        public async Task<string> Login(LoginAdminDriverRequest request)
        {
            AdminModel model = await _repository.GetByEmail(request.Email);

            if (model == null)
                throw new Exception($"Admin with E-mail = {request.Email} not found");

            if (model.Email != request.Email)
                throw new Exception("E-mail not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, model.Password))
                throw new Exception("Wrong password");

            return CreateToken(model);
        }

        public async Task<bool> UpdatePassword(string id, PasswordRequest request)
        {
            ValidatorAdminDriver.Password(request.OldPassword);
            AdminModel model = await GetByIdModel(id);

            if (model == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, model.Password))
                return false;

            model = PasswordRequest.ConvertAdmin(model, request);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        private string CreateToken(AdminModel model)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogDebug($"Admin with Id = {model} logged");

            return jwt;
        }

        public Task<AdminModel> GetByIdModel(string id)
        {
            return _repository.GetById(id) ?? throw new Exception($"Admin with Id = {id} not found");
        }

        public Task<AdminModel> GetByEmailModel(string email)
        {
            return _repository.GetByEmail(email) ?? throw new Exception($"Admin with E-mail = {email} not found");
        }
    }
}