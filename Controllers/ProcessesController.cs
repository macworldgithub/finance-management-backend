using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessesController : ControllerBase
    {
        private readonly ProcessService _processService;

        public ProcessesController(ProcessService processService)
        {
            _processService = processService;
        }

        // ===== Single-item CRUD =====

// GET: api/processes?page=1&search=onboarding&pageSize=20&sortByNoAsc=true
[HttpGet]
public async Task<ActionResult<PagedResult<Process>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null,
    [FromQuery] int pageSize = 10,
    [FromQuery] bool sortByNoAsc = false)
{
    var result = await _processService.GetAllAsync(page, search, pageSize, sortByNoAsc);
    return Ok(result);
}


        // GET: api/processes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Process>> GetById(string id)
        {
            var item = await _processService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/processes
        [HttpPost]
        public async Task<ActionResult<Process>> Create(Process process)
        {
            var created = await _processService.CreateAsync(process);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/processes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Process updated)
        {
            var existing = await _processService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _processService.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/processes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _processService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _processService.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // ===== Bulk endpoints =====

        // POST: api/processes/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<Process>>> CreateMany(List<Process> items)
        {
            var created = await _processService.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/processes/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<Process> items)
        {
            var updatedCount = await _processService.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/processes/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _processService.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }


                // SINGLE UPDATE BY No
        // PUT: api/processes/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] Process body)
        {
            // No in URL is the key; No in body is ignored for update
            var success = await _processService.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/processes/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<Process> items)
        {
            var updatedCount = await _processService.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
