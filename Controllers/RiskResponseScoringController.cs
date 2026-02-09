using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskResponseScoringController : ControllerBase
    {
        private readonly RiskResponseScoringService _service;

        public RiskResponseScoringController(RiskResponseScoringService service)
        {
            _service = service;
        }

        // GET all with pagination & optional search
        [HttpGet]
        public async Task<ActionResult<PagedResult<RiskResponseScoring>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _service.GetAllAsync(page, search, pageSize);
            return Ok(result);
        }

        // GET by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskResponseScoring>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<RiskResponseScoring>> Create(RiskResponseScoring item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT update by Id
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, RiskResponseScoring updated)
        {
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE by Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var success = await _service.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // GET column-wise totals
        [HttpGet("totals")]
        public async Task<ActionResult<Dictionary<string, int>>> GetTotals()
        {
            var totals = await _service.GetColumnWiseSumsAsync();
            return Ok(totals);
        }
    }
}
