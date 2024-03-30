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
        public ActionResult<List<AdminDTO>> GetAll()
        {
            return Ok(_adminService.GetAll());
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public ActionResult<AdminDTO> GetById(string id)
        {
            AdminDTO dto = _adminService.GetById(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Admin with Id = {id} not found");

        }

        [HttpPost]
        public ActionResult<AdminDTO> Create([FromBody] LoginAdminDriverRequest request)
        {
            AdminDTO delivery = _adminService.Create(request);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPost("Login")]
        public ActionResult<AdminDTO> Login([FromBody] LoginAdminDriverRequest request)
        {
            return Ok(_adminService.Login(request));
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePassword(string id, PasswordRequest request)
        {
            bool successful = _adminService.UpdatePassword(id, request);
            if (successful)
                return Ok();

            return BadRequest("The old password is different");
        }
    }
}
