using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtherControlEnvironmentsController : ControllerBase
    {
        private readonly OtherControlEnvironmentService _service;

        public OtherControlEnvironmentsController(OtherControlEnvironmentService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

        // GET: api/othercontrolenvironments
        [HttpGet]
        public async Task<ActionResult<List<OtherControlEnvironment>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        // GET: api/othercontrolenvironments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OtherControlEnvironment>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/othercontrolenvironments
        [HttpPost]
        public async Task<ActionResult<OtherControlEnvironment>> Create(OtherControlEnvironment item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/othercontrolenvironments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OtherControlEnvironment updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/othercontrolenvironments/{id}
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

        // POST: api/othercontrolenvironments/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<OtherControlEnvironment>>> CreateMany(List<OtherControlEnvironment> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/othercontrolenvironments/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<OtherControlEnvironment> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/othercontrolenvironments/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/othercontrolenvironments/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] OtherControlEnvironment body)
        {
            var success = await _service.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/othercontrolenvironments/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<OtherControlEnvironment> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
