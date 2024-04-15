using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using System.Security.Claims;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repository;
        private readonly ICnhImageRepository _cnhImageRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DriverService> _logger;
        private readonly IConfiguration _configuration;

        public DriverService(IDriverRepository driverRepository, ICnhImageRepository cnhImageRepository,
            ITokenService tokenService, IHttpContextAccessor httpContextAccessor, ILogger<DriverService> logger, IConfiguration configuration)
        {
            _repository = driverRepository;
            _cnhImageRepository = cnhImageRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<DriverDTO>> GetAllAsync()
        {
            var model = _repository.GetAll();
            return await DriverDTO.Convert(model);
        }

        public async Task<DriverDTO> GetByIdAsync(string id)
        {
            var model = await _repository.GetById(id);
            if (model != null)
                return await DriverDTO.Convert(model);

            _logger.LogDebug($"Driver with Id = {id} not found");
            return null;
        }

        public async Task<List<DriverDTO>> GetByStatusAsync(DriverStatusEnum status)
        {
            var model = _repository.GetByStatus(status);
            if (model == null)
                throw new Exception("There are no drivers available");

            return await DriverDTO.Convert(model);
        }

        public async Task<DriverDTO> GetByEmailAsync(string email)
        {
            DriverModel model = await _repository.GetByEmail(email);
            if (model != null)
                return await DriverDTO.Convert(model);

            _logger.LogDebug($"Driver with E-mail = {email} not found");
            return null;
        }

        public async Task<DriverDTO> CreateAsync(DriverRequest request)
        {
            DriverModel model = DriverRequest.Convert(request);

            ValidatorAdminDriver.Password(model.Password);

            ValidatorAdminDriver.Email(model.Email);
            var email = await _repository.GetByEmail(model.Email);
            if (email != null)
            {
                _logger.LogError("The E-mail must be unique");
                throw new Exception("The E-mail must be unique");
            }

            var cnh = await _repository.GetByCnh(model.Cnh);
            if (cnh != null)
            {
                _logger.LogError("The CNH must be unique");
                throw new Exception("The CNH must be unique");
            }

            ValidatorAdminDriver.Cnpj(model.Cnpj);
            var cnpj = await _repository.GetByCnpj(model.Cnpj);
            if (cnpj != null)
            {
                _logger.LogError("The CNPJ must be unique");
                throw new Exception("The CNPJ must be unique");
            }

            await _repository.CreateAsync(model);
            return await DriverDTO.Convert(model);
        }

        public async Task<bool> UpdateAsync(string id, DriverUpdateRequest request)
        {
            var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DriverModel model = await GetByIdModel(nameIdentifier);

            if (id != model.Id)
            {
                _logger.LogError($"Error ID = {id} != ID = {model.Id}");
                throw new Exception($"Error ID = {id} != ID = {model.Id}");
            }

            if (model.Email != request.Email)
            {
                ValidatorAdminDriver.Email(request.Email);
                var emailvalidate = await _repository.GetByEmail(request.Email);
                if (emailvalidate != null)
                {
                    _logger.LogError("The E-mail must be unique");
                    throw new Exception("The E-mail must be unique");
                }
            }

            if (model.Cnh != request.Cnh)
            {
                var cnh = await _repository.GetByCnh(request.Cnh);
                if (cnh != null)
                {
                    _logger.LogError("The CNH must be unique");
                    throw new Exception("The CNH must be unique");
                }
            }

            if (model.Cnpj != request.Cnpj)
            {
                ValidatorAdminDriver.Cnpj(request.Cnpj);
                var cnpj = await _repository.GetByCnpj(request.Cnpj);
                if (cnpj != null)
                {
                    _logger.LogError("The CNPJ must be unique");
                    throw new Exception("The CNPJ must be unique");
                }
            }

            model = DriverUpdateRequest.Convert(model, request);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<bool> UpdatePasswordAsync(string id, PasswordRequest request)
        {
            ValidatorAdminDriver.Password(request.OldPassword);

            var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DriverModel model = await GetByIdModel(nameIdentifier);

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, model.Password))
            {
                _logger.LogDebug("The old password is incorrect");
                return false;
            }

            model = PasswordRequest.ConvertDriver(model, request);
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<bool> UpdateStatus(string id, DriverStatusEnum status)
        {
            DriverModel model = await GetByIdModel(id);

            model.Status = status;
            await _repository.UpdateAsync(id, model);
            return true;
        }

        public async Task<TokenDTO> LoginAsync(LoginRequest request)
        {
            DriverModel model = await GetByEmailModel(request.Email);

            if (model.Email != request.Email)
                throw new Exception("E-mail not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, model.Password))
                throw new Exception("Wrong password");

            return _tokenService.CreateToken(model.Id, model.Email, model.Rule);
        }

        public async Task<bool> UploadCnhImageAsync([FromForm] IFormFile image)
        {
            var nameIdentifier = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DriverModel model = await GetByIdModel(nameIdentifier);

            if (image != null && image.Length > 0)
            {
                if (image.ContentType != "image/png" && image.ContentType != "image/bmp")
                    throw new Exception("The file format must be PNG or BMP");

                string storagePath = _configuration["CnhImageStoragePath"];

                var filePath = Path.Combine(storagePath, image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                CnhImageModel cnhImage = CnhImageRequest.Convert(image.FileName, filePath, image.Length);

                await _cnhImageRepository.CreateAsync(cnhImage);
                model.CnhImageId = cnhImage.Id;
                await _repository.UpdateAsync(model.Id, model);

                return true;
            }
            return false;
        }

        public async Task<DriverModel> GetByIdModel(string id)
        {
            return await _repository.GetById(id) ?? throw new Exception($"Driver with Id = {id} not found");
        }

        public async Task<DriverModel> GetByEmailModel(string email)
        {
            return await _repository.GetByEmail(email) ?? throw new Exception($"Driver with E-mail = {email} not found");
        }
    }
}