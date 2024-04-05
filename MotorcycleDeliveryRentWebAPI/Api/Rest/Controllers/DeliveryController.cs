using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<ActionResult<DeliveryDTO>> GetAll()
        {
            var deliverys = await _deliveryService.GetAllAsync();
            return Ok(deliverys);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryDTO>> GetById(string id)
        {
            DeliveryDTO dto = await _deliveryService.GetByIdAsync(id);
            if (dto != null)
                return Ok(dto);

            return NotFound($"Delivery id = {id} not found");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeliveryDTO>> Create([FromBody] decimal price)
        {
            DeliveryDTO delivery = await _deliveryService.CreateAsync(price);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPut("Accept/{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult> Accept(string id)
        {
            bool successful = await _deliveryService.AcceptAsync(id);
            if (successful)
                return Ok(new { message = "Race accepted" });
            return NotFound();
        }

        [HttpPut("Delivery/{id}"), Authorize(Roles = "User")]
        public async Task<ActionResult> Delivery(string id)
        {
            bool successful = await _deliveryService.DeliveryAsync(id);
            if (successful)
                return Ok(new { message = "Race delivered" });

            return NotFound();
        }

        [HttpPut("Cancel/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> Cancel(string id)
        {
            bool successful = await _deliveryService.CancelAsync(id);
            if (successful)
                return Ok(new { message = "Race Canceled" });

            return BadRequest("A race has already ended or been canceled");
        }
    }
}
