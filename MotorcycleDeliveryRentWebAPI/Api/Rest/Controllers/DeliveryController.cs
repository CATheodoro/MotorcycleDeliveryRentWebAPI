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
        public ActionResult<List<DeliveryDTO>> GetAll()
        {
            return Ok(_deliveryService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<DeliveryDTO> GetById(string id)
        {
            DeliveryDTO dto = _deliveryService.GetById(id);
            if (dto != null)
                return Ok();

            return NotFound($"Delivery id = {id} not found");
        }

        [HttpGet("Notification")]
        public ActionResult<DeliveryDTO> GetByDriverNotification()
        {
            return Ok(_deliveryService.GetByDriverNotification());
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult<DeliveryDTO> Create([FromBody] decimal price)
        {
            DeliveryDTO delivery = _deliveryService.Create(price);
            return CreatedAtAction(nameof(GetAll), new { id = delivery.Id }, delivery);
        }

        [HttpPut("Accept/{id}"), Authorize(Roles = "User")]
        public ActionResult Accept(string id)
        {
            bool successful = _deliveryService.Accept(id);
            if (successful)
                return Ok(new { message = "Race accepted" });
            return NotFound();
        }

        [HttpPut("Delivery/{id}")]
        public ActionResult Delivery(string id)
        {
            bool successful = _deliveryService.Delivery(id);
            if (successful)
                return Ok(new { message = "Race delivered" });

            return NotFound();
        }

        [HttpPut("Cancel/{id}")]
        public ActionResult Cancel(string id)
        {
            bool successful = _deliveryService.Cancel(id);
            if (successful)
                return Ok(new { message = "Race Canceled" });

            return NotFound();
        }
    }
}
