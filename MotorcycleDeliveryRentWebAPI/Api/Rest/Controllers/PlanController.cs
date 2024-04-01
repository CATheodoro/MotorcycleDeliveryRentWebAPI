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
        public async Task<ActionResult<List<PlanDTO>>> Get()
        {
            var model = await _planService.GetAllAsync();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanDTO>> GetById(string id)
        {
            var model = await _planService.GetByIdAsync(id);
            if (model != null)
                return Ok(model);

            return NotFound($"Plan with Id = {id} not found");
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<PlanDTO>> Create([FromBody] PlanRequest request)
        {
            PlanDTO plan = await _planService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(string id, [FromBody] PlanRequest request)
        {
            var model = await _planService.UpdateAsync(id, request);
            if (model)
                return Ok(new { message = "Plan updated successfully." });

            return NotFound("Plan with Id = {id} not found");
        }
    }
}
