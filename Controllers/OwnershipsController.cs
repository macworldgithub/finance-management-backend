using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnershipsController : ControllerBase
    {
        private readonly OwnershipService _ownershipService;

        public OwnershipsController(OwnershipService ownershipService)
        {
            _ownershipService = ownershipService;
        }

        // ===== Single-item CRUD =====

// GET: api/ownerships?page=1&search=onboarding
[HttpGet]
public async Task<ActionResult<PagedResult<Ownership>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null)
{
    var result = await _ownershipService.GetAllAsync(page, search);
    return Ok(result);
}

        // GET: api/ownerships/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Ownership>> GetById(string id)
        {
            var item = await _ownershipService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/ownerships
        [HttpPost]
        public async Task<ActionResult<Ownership>> Create(Ownership ownership)
        {
            var created = await _ownershipService.CreateAsync(ownership);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/ownerships/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Ownership updated)
        {
            var existing = await _ownershipService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _ownershipService.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/ownerships/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _ownershipService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _ownershipService.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }

        // ===== Bulk endpoints =====

        // POST: api/ownerships/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<Ownership>>> CreateMany(List<Ownership> items)
        {
            var created = await _ownershipService.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/ownerships/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<Ownership> items)
        {
            var updatedCount = await _ownershipService.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/ownerships/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _ownershipService.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/ownerships/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] Ownership body)
        {
            var success = await _ownershipService.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/ownerships/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<Ownership> items)
        {
            var updatedCount = await _ownershipService.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
