using Microsoft.Extensions.Logging;
using Moq;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Models;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Repositories.Interfaces;
using MotorcycleDeliveryRentWebAPI.Domain.Services;
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
