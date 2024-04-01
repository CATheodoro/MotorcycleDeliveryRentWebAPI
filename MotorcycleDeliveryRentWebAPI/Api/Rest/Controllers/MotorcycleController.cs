using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        public MotorcycleController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MotorcycleDTO>>> GetAll()
        {
            var dto = await _motorcycleService.GetAllAsync();
            return Ok(dto);
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<MotorcycleDTO>> GetById(string id)
        {
            var dto = await _motorcycleService.GetByIdAsync(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Motorcycle with Id = {id} not found");
        }


        [HttpGet("Available")]
        public async Task<ActionResult<MotorcycleDTO>> GetAvailable()
        {
            var dto = await _motorcycleService.GetFirstAvailableAsync();
            return Ok(dto); ;
        }


        [HttpGet("plate/{plate}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<MotorcycleDTO>> GetByPlate(string plate)
        {
            var dto = await _motorcycleService.GetByPlateAsync(plate);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Motorcycle with plate = {plate} not found");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<MotorcycleDTO>> Create([FromBody] MotorcycleRequest Model)
        {
            var dto = await _motorcycleService.CreateAsync(Model);
            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(string id, [FromBody] MotorcycleUpdateRequest motorbike)
        {
            bool successful = await _motorcycleService.UpdateAsync(id, motorbike);
            if (successful)
                return Ok(new { message = "Motorcycle updated successfully." });

            return NotFound($"Motorcycle with id = {id} not found");
        }

        [HttpPut("plate/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdatePlate(string id, [FromBody] string plate)
        {
            bool successful = await _motorcycleService.UpdatePlateAsync(id, plate);
            if (successful)
                return Ok(new { message = "Motorcycle Plate updated successfully." });

            return BadRequest();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            bool successful = await _motorcycleService.DeleteAsync(id);
            if (successful)
                return Ok($"Motorbike with Id = {id} delete");

            return NotFound("Motorbike with Id = {id} not found");
        }
    }
}
