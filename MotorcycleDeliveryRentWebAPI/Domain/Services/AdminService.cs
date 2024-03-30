using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
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
    public class AdminService : IAdminService
    {
        private readonly IMongoCollection<AdminModel> _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminModel> _logger;

        public AdminService(IMongoDBSettings settings, IMongoClient mongoClient, IConfiguration configuration, ILogger<AdminModel> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _context = database.GetCollection<AdminModel>("Admin");
            _configuration = configuration;
            _logger = logger;
        }

        public List<AdminDTO> GetAll()
        {
            return AdminDTO.Convert(_context.Find(x => true).ToList());
        }

        public AdminDTO GetById(string id)
        {
            AdminModel model = _context.Find(x => x.Id == id).FirstOrDefault();
            if (model != null)
                return AdminDTO.Convert(model);

            _logger.LogDebug($"Admin with Id = {id} not found");
            return null;
        }

        public AdminDTO GetByEmail(string email)
        {

            AdminModel model = _context.Find(x => x.Email == email).FirstOrDefault();
            if (model != null)
                return AdminDTO.Convert(model);

            _logger.LogError($"Admin with E-mail = {email} not found");
            throw new Exception($"Admin with E-mail = {email} not found");
        }

        public AdminDTO Create(LoginAdminDriverRequest request)
        {
            ValidatorAdminDriver.Password(request.Password);
            ValidatorAdminDriver.Email(request.Email);
            if (_context.Find(delivery => delivery.Email == request.Email).FirstOrDefault() != null)
            {
                _logger.LogError($"The E-mail must be unique, E-mail = {request.Email}");
                throw new Exception($"The E-mail must be unique, E-mail = {request.Email}");
            }

            AdminModel adminModel = LoginAdminDriverRequest.ConvertAdmin(request);

            _context.InsertOne(adminModel);
            return AdminDTO.Convert(adminModel);
        }

        public string Login(LoginAdminDriverRequest request)
        {
            AdminModel model = _context.Find(x => x.Email == request.Email).FirstOrDefault() ?? throw new Exception($"Admin with E-mail = {request.Email} not found");

            if (model.Email != request.Email)
                throw new Exception("E-mail not found");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, model.Password))
                throw new Exception("Wrong password");

            return CreateToken(model);
        }

        public bool UpdatePassword(string id, PasswordRequest request)
        {
            ValidatorAdminDriver.Password(request.OldPassword);
            AdminModel model = GetByIdModel(id);

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, model.Password))
                return false;

            model = PasswordRequest.ConvertAdmin(model, request);
            _context.ReplaceOne(x => x.Id == id, model);
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

        public AdminModel GetByIdModel(string id)
        {
            return _context.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception($"Admin with Id = {id} not found");
        }

        public AdminModel GetByEmailModel(string email)
        {
            return _context.Find(x => x.Email == email).FirstOrDefault() ?? throw new Exception($"Admin with E-mail = {email} not found");
        }
    }
}