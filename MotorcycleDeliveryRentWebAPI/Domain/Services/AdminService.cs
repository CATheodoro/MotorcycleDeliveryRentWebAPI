using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IAdminRepository adminRepository, ITokenService tokenService, ILogger<AdminService> logger)
        {
            _repository = adminRepository;
            _tokenService = tokenService;
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

        public async Task<AdminDTO> CreateAsync(AdminRequest request)
        {
            ValidatorAdminDriver.Password(request.Password);
            ValidatorAdminDriver.Email(request.Email);
            var email = await GetByEmailAsync(request.Email);

            if (email != null)
            {
                _logger.LogError($"The E-mail must be unique, E-mail = {request.Email}");
                throw new Exception($"The E-mail must be unique, E-mail = {request.Email}");
            }

            AdminModel model = AdminRequest.Convert(request);
            await _repository.CreateAsync(model);
            return await AdminDTO.Convert(model);
        }

        public async Task<TokenDTO> Login(LoginRequest request)
        {
            AdminModel model = await _repository.GetByEmail(request.Email);

            if (model == null)
                throw new Exception($"Admin with E-mail = {request.Email} not found");

            if (model.Email != request.Email)
                throw new Exception("E-mail not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, model.Password))
                throw new Exception("Wrong password");

            return _tokenService.CreateToken(model.Id, model.Email, model.Rule);
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