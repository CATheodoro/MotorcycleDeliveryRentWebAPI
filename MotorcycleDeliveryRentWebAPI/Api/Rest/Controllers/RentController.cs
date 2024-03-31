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
        public ActionResult<List<RentDTO>> GetAll()
        {
            return Ok(_rentService.GetAll());
        }


        [HttpGet("{id}"), Authorize(Roles = "User")]
        public ActionResult<List<RentDTO>> GetById(string id)
        {
            RentDTO dto = _rentService.GetById(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Rent with Id = {id} not found");
        }

        [HttpGet("Driver/{driverId}"), Authorize(Roles = "User")]
        public ActionResult<List<RentDTO>> GetByDriverId(string driverId)
        {
            List<RentDTO> dto = _rentService.GetByDriverId(driverId);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Rent with DriverId = {driverId} not found");
        }

        [HttpPost("{planId}"), Authorize(Roles = "User")]
        public ActionResult<RentDTO> Create(string planId)
        {
            RentDTO model = _rentService.Create(planId);

            return CreatedAtAction(nameof(GetAll), new { id = model.Id }, model);
        }

        [HttpPut("{id}"), Authorize(Roles = "User")]
        public ActionResult Update(string id)
        {
            if (_rentService.Update(id))
                return Ok(new { message = "Rent updated successfully" });

            return NotFound($"Rent with Id = {id} not found");
        }
    }
}
