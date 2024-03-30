using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Enums;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Api.Validators;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using MotorcycleDeliveryRentWebAPI.Infra.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotorcycleDeliveryRentWebAPI.Domain.Services
{
    public class DriverService : IDriverService
    {
        private readonly IMongoCollection<DriverModel> _context;
        private readonly IMongoCollection<CnhImageModel> _contextImage;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DriverModel> _logger;

        public DriverService(IMongoDBSettings settings, IMongoClient mongoClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<DriverModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<DriverModel>("Driver");
            _contextImage = database.GetCollection<CnhImageModel>("CnhImage");

            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public List<DriverDTO> GetAll()
        {
            return DriverDTO.Convert(_context.Find(x => true).ToList());
        }

        public DriverDTO GetById(string id)
        {
            DriverModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return DriverDTO.Convert(model);

            _logger.LogDebug($"Driver with Id = {id} not found");
            return null;
        }

        public List<DriverDTO> GetByStatus(DriverStatusEnum status)
        {
            return DriverDTO.Convert(_context.Find(x => x.Status == status).ToList());
        }

        public DriverDTO GetByEmail(string email)
        {
            DriverModel model = _context.Find(x => x.Email == email).FirstOrDefault();
            if (model != null)
                return DriverDTO.Convert(model);

            _logger.LogDebug($"Driver with E-mail = {email} not found");
            return null;
        }

        public DriverDTO Create(DriverRequest request)
        {
            DriverModel model = DriverRequest.Convert(request);
            CheckDelivery(model);
            _context.InsertOne(model);
            return DriverDTO.Convert(model);
        }

        public bool Update(string id, DriverUpdateRequest request)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel model = GetByEmailModel(email);

            if (model.Email != request.Email)
            {
                ValidatorAdminDriver.Email(request.Email);
                if (_context.Find(x => x.Email == request.Email).FirstOrDefault() != null)
                {
                    _logger.LogError("The E-mail must be unique");
                    throw new Exception("The E-mail must be unique");
                }
            }

            if (model.Cnh != request.Cnh)
            {
                if (_context.Find(x => x.Cnh == request.Cnh).FirstOrDefault() != null)
                {
                    _logger.LogError("The CNH must be unique");
                    throw new Exception("The CNH must be unique");
                }
            }

            if (model.Cnpj != request.Cnpj)
            {
                ValidatorAdminDriver.Cnpj(request.Cnpj);
                if (_context.Find(x => x.Cnpj == request.Cnpj).FirstOrDefault() != null)
                {
                    _logger.LogError("The CNPJ must be unique");
                    throw new Exception("The CNPJ must be unique");
                }
            }

            model = DriverUpdateRequest.Convert(model, request);
            _context.ReplaceOne(x => x.Id == id, model);
            return true;
        }

        public bool UpdatePassword(string id, PasswordRequest request)
        {
            ValidatorAdminDriver.Password(request.OldPassword);

            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel model = GetByEmailModel(email);

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, model.Password))
            {
                _logger.LogDebug("The old password is incorrect");
                return false;
            }

            model = PasswordRequest.ConvertDriver(model, request);
            _context.ReplaceOne(x => x.Id == id, model);
            return true;
        }

        public bool UpdateStatus(string id, DriverStatusEnum status)
        {
            DriverModel model = GetByIdModel(id);

            model.Status = status;
            _context.ReplaceOne(x => x.Id == id, model);
            return true;
        }

        public string Login(LoginAdminDriverRequest request)
        {
            DriverModel model = GetByEmailModel(request.Email);

            if (model.Email != request.Email)
                throw new Exception("E-mail not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, model.Password))
                throw new Exception("Wrong password");

            return CreateToken(model);
        }

        private string CreateToken(DriverModel model)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogDebug($"Driver with Id = {model} logged");

            return jwt;
        }

        public void CheckDelivery(DriverModel model)
        {
            ValidatorAdminDriver.Email(model.Email);
            if (_context.Find(x => x.Email == model.Email).FirstOrDefault() != null)
            {
                _logger.LogError("The E-mail must be unique");
                throw new Exception("The E-mail must be unique");
            }

            if (_context.Find(x => x.Cnh == model.Cnh).FirstOrDefault() != null)
            {
                _logger.LogError("The CNH must be unique");
                throw new Exception("The CNH must be unique");
            }

            ValidatorAdminDriver.Cnpj(model.Cnpj);
            if (_context.Find(x => x.Cnpj == model.Cnpj).FirstOrDefault() != null)
            {
                _logger.LogError("The CNPJ must be unique");
                throw new Exception("The CNPJ must be unique");
            }

            ValidatorAdminDriver.Password(model.Password);
        }

        public bool UploadCnhImage([FromForm] IFormFile image)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            DriverModel model = GetByEmailModel(email);

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

                _contextImage.InsertOne(cnhImage);
                model.CnhImageId = cnhImage.Id;
                _context.ReplaceOne(x => x.Id == model.Id, model);

                return true;
            }
            return false;
        }

        public DriverModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Driver with Id = {id} not found");
        }

        public DriverModel GetByEmailModel(string email)
        {
            return _context.Find(x => x.Email == email).FirstOrDefault() ?? throw new Exception($"Driver with E-mail = {email} not found");
        }
    }
}