using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Requests;
using MotorcycleDeliveryRentWebAPI.Api.Rest.Responses;
using MotorcycleDeliveryRentWebAPI.Domain.Services.Interfaces;

namespace MotorcycleDeliveryRentWebAPI.Api.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {

        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public ActionResult<List<PlanDTO>> Get()
        {
            return Ok(_planService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<PlanDTO> GetById(string id)
        {
            PlanDTO model = _planService.GetById(id);
            if (model != null)
                return Ok(model);

            return NotFound($"Plan with Id = {id} not found");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public ActionResult<PlanDTO> Create([FromBody] PlanRequest request)
        {
            PlanDTO plan = _planService.Create(request);
            return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public ActionResult Update(string id, [FromBody] PlanRequest request)
        {
            if (_planService.Update(id, request))
                return Ok(new { message = "Plan updated successfully." });

            return NotFound("Plan with Id = {id} not found");
        }
    }
}
