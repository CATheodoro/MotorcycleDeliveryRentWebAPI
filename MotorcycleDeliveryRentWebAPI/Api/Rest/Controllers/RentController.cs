using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<RentDTO>>> GetAll()
        {
            var dto = await _rentService.GetAllAsync();
            return Ok(dto);
        }

        [HttpGet("{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult<List<RentDTO>>> GetById(string id)
        {
            RentDTO dto = await _rentService.GetByIdAsync(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Rent with Id = {id} not found");
        }

        [HttpGet("Driver/{driverId}"), Authorize(Roles = "User")]
        public async Task<ActionResult<List<RentDTO>>> GetByDriverId(string driverId)
        {
            List<RentDTO> dto = await _rentService.GetByDriverIdAsync(driverId);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Rent with DriverId = {driverId} not found");
        }

        [HttpPost("{planId}"), Authorize(Roles = "User")]
        public async Task<ActionResult<RentDTO>> Create(string planId)
        {
            var model = await _rentService.CreateAsync(planId);
            return CreatedAtAction(nameof(GetAll), new { id = model.Id }, model);
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult> Update(string id)
        {
            var successful = await _rentService.UpdateAsync(id);
            if (successful)
                return Ok(new { message = "Rent updated successfully" });

            return NotFound($"Rent with Id = {id} not found");
        }
    }
}
