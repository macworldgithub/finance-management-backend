using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoxController : ControllerBase
    {
        private readonly SoxService _service;

        public SoxController(SoxService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

     // GET: api/sox?page=1&search=revenue
[HttpGet]
public async Task<ActionResult<PagedResult<Sox>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null)
{
    var result = await _service.GetAllAsync(page, search);
    return Ok(result);
}


        // GET: api/sox/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sox>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/sox
        [HttpPost]
        public async Task<ActionResult<Sox>> Create(Sox item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/sox/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Sox updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/sox/{id}
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

        // POST: api/sox/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<Sox>>> CreateMany(List<Sox> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/sox/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<Sox> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/sox/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/sox/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] Sox body)
        {
            var success = await _service.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/sox/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<Sox> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
