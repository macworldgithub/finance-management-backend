using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntosaiIfacControlEnvironmentsController : ControllerBase
    {
        private readonly IntosaiIfacControlEnvironmentService _service;

        public IntosaiIfacControlEnvironmentsController(IntosaiIfacControlEnvironmentService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

 // GET: api/intosaifaccontrolenvironments?page=1&search=integrity&pageSize=20&sortByNoAsc=true
[HttpGet]
public async Task<ActionResult<PagedResult<IntosaiIfacControlEnvironment>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null,
    [FromQuery] int pageSize = 10,
    [FromQuery] bool sortByNoAsc = false)
{
    var result = await _service.GetAllAsync(page, search, pageSize, sortByNoAsc);
    return Ok(result);
}


        // GET: api/intosaifaccontrolenvironments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IntosaiIfacControlEnvironment>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/intosaifaccontrolenvironments
        [HttpPost]
        public async Task<ActionResult<IntosaiIfacControlEnvironment>> Create(IntosaiIfacControlEnvironment item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/intosaifaccontrolenvironments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, IntosaiIfacControlEnvironment updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/intosaifaccontrolenvironments/{id}
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

        // POST: api/intosaifaccontrolenvironments/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<IntosaiIfacControlEnvironment>>> CreateMany(List<IntosaiIfacControlEnvironment> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/intosaifaccontrolenvironments/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<IntosaiIfacControlEnvironment> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/intosaifaccontrolenvironments/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/intosaifaccontrolenvironments/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] IntosaiIfacControlEnvironment body)
        {
            var success = await _service.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/intosaifaccontrolenvironments/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<IntosaiIfacControlEnvironment> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
