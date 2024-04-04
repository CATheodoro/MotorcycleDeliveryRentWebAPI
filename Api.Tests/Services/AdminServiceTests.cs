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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Tests.Services
{
    public class AdminServiceTests
    {
        Mock<ILogger<AdminService>> _adminLoggerMock = new Mock<ILogger<AdminService>>();
        Mock<IAdminRepository> _adminRepositoryMock = new Mock<IAdminRepository>();
        Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        Mock<ITokenService> _TokenService = new Mock<ITokenService>();

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

            var newAdmin = new AdminRequest
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
            var loginRequest = new LoginRequest
            {
                Email = "admin1@example.com",
                Password = "Password@123"
            };

            var adminModel = new AdminModel
            {
                Id = "660ac4ad9c1212e693221c0f",
                Email = "admin1@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password@123"),
                Rule = new List<string> { "Admin" }
            };

            var returnToken = new TokenDTO(
                email: "admin1@example.com",
                roles: new List<string> { "Admin" },
                accessToken: "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjY2MGYwYjk2ZTE2ZDBkNzIyYTlkNzI1ZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImVtYWlsMTIzMzIxQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsInRlc3RlIl0sImV4cCI6MTcxMjM1MTgwOH0.GriOif3YmpwVPih3l86trmD9U1L1bxxPPE_JhTXp9Jef3kylGKGGd2CfZlkb56jaJ4BoGIz2pqsoXss0SIFpoQ"
            );

            _configuration.Setup(x => x.GetSection("AppSettings:Token").Value)
                         .Returns("bwO5R0iQNZt+9NVIt4AeQplO9JysBrs/Ugr/d7l0j3g=bwO5R0iQNZt+9NVIt4AeQplO9JysBrs/Ugr/d7l0j3g=");

            _adminRepositoryMock.Setup(repo => repo.GetByEmail(loginRequest.Email)).ReturnsAsync(adminModel);

            _TokenService.Setup(x => x.CreateToken(adminModel.Id, adminModel.Email, adminModel.Rule)).Returns(returnToken);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, _TokenService.Object, _adminLoggerMock.Object);

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

            var loginRequest = new LoginRequest
            {
                Email = "admin1@example.com",
                Password = "Password@123"
            };

            var adminRequest = new AdminRequest
            {
                Email = "admin1@example.com",
                Password = "Password@123"
            };

            _adminRepositoryMock.Setup(repo => repo.GetByEmail(loginRequest.Email)).ReturnsAsync(adminModel);

            AdminService adminService = new AdminService(_adminRepositoryMock.Object, null, _adminLoggerMock.Object);

            var exception = await Assert.ThrowsAsync<Exception>(async () => await adminService.CreateAsync(adminRequest));
            Assert.Equal("The E-mail must be unique, E-mail = admin1@example.com", exception.Message);
        }
    }
}
