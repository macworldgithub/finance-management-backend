using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentOfAdequacyController : ControllerBase
    {
        private readonly AssessmentOfAdequacyService _service;

        public AssessmentOfAdequacyController(AssessmentOfAdequacyService service)
        {
            _service = service;
        }

        // GET: api/assessmentofadequacy?page=1&search=Adequate&pageSize=20
        [HttpGet]
        public async Task<ActionResult<PagedResult<AssessmentOfAdequacy>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] string? search = null,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllAsync(page, search, pageSize);
            return Ok(result);
        }

        // GET: api/assessmentofadequacy/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentOfAdequacy>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/assessmentofadequacy
        [HttpPost]
        public async Task<ActionResult<AssessmentOfAdequacy>> Create(AssessmentOfAdequacy item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/assessmentofadequacy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AssessmentOfAdequacy updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/assessmentofadequacy/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // ===== Bulk Endpoints =====

        [HttpPost("bulk")]
        public async Task<ActionResult<List<AssessmentOfAdequacy>>> CreateMany(List<AssessmentOfAdequacy> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // You can add UpdateMany / DeleteMany if needed later
    }
}