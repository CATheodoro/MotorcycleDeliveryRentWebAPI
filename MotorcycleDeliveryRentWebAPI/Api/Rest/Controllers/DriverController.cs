using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DriverDTO>>> GetAll()
        {
            var drivers = await _driverService.GetAllAsync();
            return Ok(drivers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverDTO>> GetById(string id)
        {
            DriverDTO dto = await _driverService.GetByIdAsync(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Driver with Id = {id} not found");
        }

        [HttpPost]
        public async Task<ActionResult<DriverDTO>> Create([FromBody] DriverRequest request)
        {
            DriverDTO delivery = await _driverService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginRequest request)
        {
            var dto = await _driverService.LoginAsync(request);
            return Ok(dto);
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult> Update(string id, [FromBody] DriverUpdateRequest request)
        {
            bool successful = await _driverService.UpdateAsync(id, request);
            if (successful)
                return Ok(new { message = "Driver updated successfully" });

            return NotFound($"Driver with Id = {id} not found");
        }

        [HttpPut("Password/{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult> UpdatePassword(string id, [FromBody] PasswordRequest request)
        {
            bool successful = await _driverService.UpdatePasswordAsync(id, request);
            if (successful)
                return Ok(new { message = "Password updated successfully" });

            return NotFound("The old password is incorrect");
        }

        [HttpPost("cnh/upload"), Authorize(Roles = "User")]
        public async Task<ActionResult> UploadCnhImage([FromForm] IFormFile image)
        {
            bool updateSuccessful = await _driverService.UploadCnhImageAsync(image);
            if (updateSuccessful)
                return Ok(new { message = "Image of driver's license was sent successfully" });

            return BadRequest("No images were sent");
        }
    }
}
