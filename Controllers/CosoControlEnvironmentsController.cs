using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CosoControlEnvironmentsController : ControllerBase
    {
        private readonly CosoControlEnvironmentService _cosoService;

        public CosoControlEnvironmentsController(CosoControlEnvironmentService cosoService)
        {
            _cosoService = cosoService;
        }

        // ===== Single-item CRUD =====

      // GET: api/cosocontrolenvironments?page=1&search=board
[HttpGet]
public async Task<ActionResult<PagedResult<CosoControlEnvironment>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null)
{
    var result = await _cosoService.GetAllAsync(page, search);
    return Ok(result);
}

        // GET: api/cosocontrolenvironments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CosoControlEnvironment>> GetById(string id)
        {
            var item = await _cosoService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/cosocontrolenvironments
        [HttpPost]
        public async Task<ActionResult<CosoControlEnvironment>> Create(CosoControlEnvironment item)
        {
            var created = await _cosoService.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/cosocontrolenvironments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CosoControlEnvironment updated)
        {
            var existing = await _cosoService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _cosoService.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/cosocontrolenvironments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _cosoService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _cosoService.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // ===== Bulk endpoints =====

        // POST: api/cosocontrolenvironments/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<CosoControlEnvironment>>> CreateMany(List<CosoControlEnvironment> items)
        {
            var created = await _cosoService.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/cosocontrolenvironments/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<CosoControlEnvironment> items)
        {
            var updatedCount = await _cosoService.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/cosocontrolenvironments/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _cosoService.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/cosocontrolenvironments/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] CosoControlEnvironment body)
        {
            var success = await _cosoService.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/cosocontrolenvironments/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<CosoControlEnvironment> items)
        {
            var updatedCount = await _cosoService.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
