using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentOfEfficiencyController : ControllerBase
    {
        private readonly AssessmentOfEfficiencyService _service;

        public AssessmentOfEfficiencyController(AssessmentOfEfficiencyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<AssessmentOfEfficiency>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] string? search = null,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(page, search, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentOfEfficiency>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<AssessmentOfEfficiency>> Create(AssessmentOfEfficiency item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AssessmentOfEfficiency updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<List<AssessmentOfEfficiency>>> CreateMany(List<AssessmentOfEfficiency> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }
    }
}