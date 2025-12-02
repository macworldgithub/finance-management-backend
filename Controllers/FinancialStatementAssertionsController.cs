using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialStatementAssertionsController : ControllerBase
    {
        private readonly FinancialStatementAssertionService _service;

        public FinancialStatementAssertionsController(FinancialStatementAssertionService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

      // GET: api/financialstatementassertions?page=1&search=revenue
// GET: api/financialstatementassertions?page=1&search=revenue&pageSize=20&sortByNoAsc=true
[HttpGet]
public async Task<ActionResult<PagedResult<FinancialStatementAssertion>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null,
    [FromQuery] int pageSize = 10,
    [FromQuery] bool sortByNoAsc = false)
{
    var result = await _service.GetAllAsync(page, search, pageSize, sortByNoAsc);
    return Ok(result);
}



        // GET: api/financialstatementassertions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialStatementAssertion>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/financialstatementassertions
        [HttpPost]
        public async Task<ActionResult<FinancialStatementAssertion>> Create(FinancialStatementAssertion item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/financialstatementassertions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, FinancialStatementAssertion updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/financialstatementassertions/{id}
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

        // POST: api/financialstatementassertions/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<FinancialStatementAssertion>>> CreateMany(
            List<FinancialStatementAssertion> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/financialstatementassertions/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<FinancialStatementAssertion> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/financialstatementassertions/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/financialstatementassertions/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] FinancialStatementAssertion body)
        {
            var success = await _service.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/financialstatementassertions/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo(
            [FromBody] List<FinancialStatementAssertion> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
