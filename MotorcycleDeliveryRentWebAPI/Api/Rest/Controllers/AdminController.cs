using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<AdminDTO>>> GetAll()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdminDTO>> GetById(string id)
        {
            var dto = await _adminService.GetByIdAsync(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Admin with Id = {id} not found");
        }

        [HttpPost]
        public async Task<ActionResult<AdminDTO>> Create([FromBody] LoginAdminDriverRequest request)
        {
            var delivery = await _adminService.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginAdminDriverRequest request)
        {
            var dto = await _adminService.Login(request);
            return Ok(dto);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePassword(string id, PasswordRequest request)
        {
            var successful = await _adminService.UpdatePassword(id, request);
            if (successful)
                return Ok(new { message = "Password updated" });

            return BadRequest("The old password is different");
        }
    }
}
