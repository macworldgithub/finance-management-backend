using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtherControlEnvironmentScoringsController : ControllerBase
    {
        private readonly OtherControlEnvironmentScoringService _service;

        public OtherControlEnvironmentScoringsController(OtherControlEnvironmentScoringService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OtherControlEnvironmentScoring>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] string? search = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool sortByNoAsc = false)
        {
            var result = await _service.GetAllAsync(page, search, pageSize, sortByNoAsc);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OtherControlEnvironmentScoring>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<OtherControlEnvironmentScoring>> Create(OtherControlEnvironmentScoring item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, OtherControlEnvironmentScoring updated)
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
        public async Task<ActionResult<List<OtherControlEnvironmentScoring>>> CreateMany(List<OtherControlEnvironmentScoring> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<OtherControlEnvironmentScoring> items)
        {
            var count = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount = count });
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var count = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount = count });
        }

        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] OtherControlEnvironmentScoring body)
        {
            var success = await _service.UpdateByNoAsync(no, body);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<OtherControlEnvironmentScoring> items)
        {
            var count = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount = count });
        }
    }
}