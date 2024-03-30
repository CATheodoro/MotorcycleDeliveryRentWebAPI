using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService deliveryService;

        public DriverController(IDriverService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        public ActionResult<List<DriverDTO>> GetAll()
        {
            return Ok(deliveryService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<DriverDTO> GetById(string id)
        {
            DriverDTO dto = deliveryService.GetById(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Driver with Id = {id} not found");
        }

        [HttpPost]
        public ActionResult<DriverDTO> Create([FromBody] DriverRequest deliveryRequestCreateModel)
        {
            DriverDTO delivery = deliveryService.Create(deliveryRequestCreateModel);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPost("Login")]
        public ActionResult<DriverDTO> Login([FromBody] LoginAdminDriverRequest request)
        {
            return Ok(deliveryService.Login(request));
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public ActionResult Update(string id, [FromBody] DriverUpdateRequest deliveryRequestUpdateModel)
        {
            bool successful = deliveryService.Update(id, deliveryRequestUpdateModel);
            if (successful)
                return Ok(new { message = "Driver updated successfully" });

            return NotFound($"Driver with Id = {id} not found");
        }

        [HttpPut("Password/{id}"), Authorize(Roles = "User")]
        public ActionResult UpdatePassword(string id, [FromBody] PasswordRequest deliveryRequestPasswordModel)
        {
            bool successful = deliveryService.UpdatePassword(id, deliveryRequestPasswordModel);
            if (successful)
                return Ok(new { message = "Password updated successfully" });

            return NotFound("The old password is incorrect");
        }

        [HttpPost("cnh/upload"), Authorize(Roles = "User")]
        public ActionResult UploadCnhImage([FromForm] IFormFile image)
        {
            bool updateSuccessful = deliveryService.UploadCnhImage(image);
            if (updateSuccessful)
                return Ok(new { message = "Image of driver's license was sent successfully" });

            return BadRequest("No images were sent");
        }
    }
}
