using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Tests.Services
{
    public class AdminServiceTests
    {
        Mock<ILogger<AdminModel>> _adminLoggerMock = new Mock<ILogger<AdminModel>>();
        Mock<IAdminRepository> _adminRepositoryMock = new Mock<IAdminRepository>();
        Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        [Fact(DisplayName = "Get all admin: return list Success")]
        public async Task GetAllAsync_ReturnsAdminDTOList()
        {
            var adminModels = new List<AdminModel>
            {
                new AdminModel { Id = "1", Email = "admin1@example.com" },
                new AdminModel { Id = "2", Email = "admin2@example.com" },
            };

            _adminRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(adminModels);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, null, null);

            var result = await adminService.GetAllAsync();

            Assert.NotNull(result);
            Assert.IsType<List<AdminDTO>>(result);
            Assert.Equal(adminModels.Count, result.Count);
        }

        [Fact(DisplayName = "Create admin Success")]
        public async Task CreateAsync_ValidRequest_ReturnsAdminDTO()
        {
            var adminModels = new List<AdminModel>
            {
                new AdminModel { Id = "1", Email = "admin1@example.com" },
                new AdminModel { Id = "2", Email = "admin2@example.com" },
            };

            _adminRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(adminModels);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, null, _adminLoggerMock.Object);

            var newAdmin = new LoginAdminDriverRequest
            {
                Email = "newAdmin@email.com",
                Password = "Password@123"
            };

            var result = await adminService.CreateAsync(newAdmin);

            Assert.NotNull(result);
            Assert.Equal(newAdmin.Email, result.Email);

        }

        [Fact(DisplayName = "Admin logged Success")]
        public async Task Login_Successful()
        {
            var loginRequest = new LoginAdminDriverRequest
            {
                Email = "admin1@example.com",
                Password = "Password@123"
            };

            var adminModel = new AdminModel
            {
                Id = "660ac4ad9c1212e693221c0f",
                Email = "admin1@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password@123")
            };

            _configuration.Setup(x => x.GetSection("AppSettings:Token").Value)
                         .Returns("bwO5R0iQNZt+9NVIt4AeQplO9JysBrs/Ugr/d7l0j3g=bwO5R0iQNZt+9NVIt4AeQplO9JysBrs/Ugr/d7l0j3g=");

            _adminRepositoryMock.Setup(repo => repo.GetByEmail(loginRequest.Email)).ReturnsAsync(adminModel);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, _configuration.Object, _adminLoggerMock.Object);

            var token = await adminService.Login(loginRequest);

            Assert.NotNull(token);
        }

        [Fact(DisplayName = "Create admin error: e-mail must be unique")]
        public async Task CreateAsync_DuplicateEmail_ThrowsException()
        {
            var adminModel = new AdminModel
            {
                Id = "1",
                Email = "admin1@example.com"
            };

            var loginAdminDriverRequest = new LoginAdminDriverRequest
            {
                Email = "admin1@example.com",
                Password = "Password@123"
            };

            _adminRepositoryMock.Setup(repo => repo.GetByEmail(loginAdminDriverRequest.Email)).ReturnsAsync(adminModel);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, null, _adminLoggerMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(async () => await adminService.CreateAsync(loginAdminDriverRequest));
            Assert.Equal("The E-mail must be unique, E-mail = admin1@example.com", exception.Message);
        }
    }
}
