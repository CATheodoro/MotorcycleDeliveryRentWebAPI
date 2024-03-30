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
        private readonly IMotorcycleService motorcycleService;

        public MotorcycleController(IMotorcycleService motorcycleService)
        {
            this.motorcycleService = motorcycleService;
        }

        [HttpGet]
        public ActionResult<List<MotorcycleDTO>> GetAll()
        {
            return Ok(motorcycleService.GetAll());
        }

        [HttpGet("{id}"), Authorize(Roles = "Admin")]
        public ActionResult<MotorcycleDTO> GetById(string id)
        {
            MotorcycleDTO dto = motorcycleService.GetById(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Motorcycle with Id = {id} not found");
        }


        [HttpGet("Available")]
        public ActionResult<MotorcycleDTO> GetAvailable()
        {
            return Ok(motorcycleService.GetAvailable()); ;
        }


        [HttpGet("plate/{plate}"), Authorize(Roles = "Admin")]
        public ActionResult<MotorcycleDTO> GetByPlate(string plate)
        {
            MotorcycleDTO dto = motorcycleService.GetByPlate(plate);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Motorcycle with plate = {plate} not found");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult<MotorcycleDTO> Create([FromBody] MotorcycleRequest Model)
        {
            MotorcycleDTO dto = motorcycleService.Create(Model);
            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public ActionResult Update(string id, [FromBody] MotorcycleUpdateRequest motorbike)
        {
            bool successful = motorcycleService.Update(id, motorbike);
            if (successful)
                return Ok(new { message = "Motorcycle updated successfully." });

            return NotFound($"Motorcycle with id = {id} not found");
        }

        [HttpPut("plate/{id}"), Authorize(Roles = "Admin")]
        public ActionResult UpdatePlate(string id, [FromBody] string plate)
        {
            bool successful = motorcycleService.UpdatePlate(id, plate);
            if (successful)
                return Ok(new { message = "Motorcycle Plate updated successfully." });

            return BadRequest();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (motorcycleService.Delete(id))
                return Ok($"Motorbike with Id = {id} delete");

            return NotFound("Motorbike with Id = {id} not found");
        }
    }
}
