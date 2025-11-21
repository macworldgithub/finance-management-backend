using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrcExceptionLogsController : ControllerBase
    {
        private readonly GrcExceptionLogService _service;

        public GrcExceptionLogsController(GrcExceptionLogService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

    // GET: api/grcexceptionlogs?page=1&search=onboarding
[HttpGet]
public async Task<ActionResult<PagedResult<GrcExceptionLog>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null)
{
    var result = await _service.GetAllAsync(page, search);
    return Ok(result);
}

        // GET: api/grcexceptionlogs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GrcExceptionLog>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/grcexceptionlogs
        [HttpPost]
        public async Task<ActionResult<GrcExceptionLog>> Create(GrcExceptionLog item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/grcexceptionlogs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, GrcExceptionLog updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/grcexceptionlogs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // ===== Bulk endpoints =====

        // POST: api/grcexceptionlogs/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<GrcExceptionLog>>> CreateMany(List<GrcExceptionLog> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/grcexceptionlogs/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<GrcExceptionLog> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/grcexceptionlogs/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // PUT: api/grcexceptionlogs/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] GrcExceptionLog body)
        {
            var success = await _service.UpdateByNoAsync(no, body);
            if (!success) return NotFound();
            return NoContent();
        }

        // PUT: api/grcexceptionlogs/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<GrcExceptionLog> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
