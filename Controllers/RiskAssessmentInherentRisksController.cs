using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RiskAssessmentInherentRisksController : ControllerBase
    {
        private readonly RiskAssessmentInherentRiskService _service;

        public RiskAssessmentInherentRisksController(RiskAssessmentInherentRiskService service)
        {
            _service = service;
        }

        // ===== Single-item CRUD =====

        // GET: api/riskassessmentinherentrisk?page=1&search=credit
[HttpGet]
public async Task<ActionResult<PagedResult<RiskAssessmentInherentRisk>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] string? search = null)
{
    var result = await _service.GetAllAsync(page, search);
    return Ok(result);
}

  

        // GET: api/riskassessmentinherentriskS/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RiskAssessmentInherentRisk>> GetById(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/riskassessmentinherentriskS
        [HttpPost]
        public async Task<ActionResult<RiskAssessmentInherentRisk>> Create(RiskAssessmentInherentRisk item)
        {
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/riskassessmentinherentriskS/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, RiskAssessmentInherentRisk updated)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _service.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/riskassessmentinherentriskS/{id}
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

        // POST: api/riskassessmentinherentriskS/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<List<RiskAssessmentInherentRisk>>> CreateMany(List<RiskAssessmentInherentRisk> items)
        {
            var created = await _service.CreateManyAsync(items);
            return Ok(created);
        }

        // PUT: api/riskassessmentinherentriskS/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateMany(List<RiskAssessmentInherentRisk> items)
        {
            var updatedCount = await _service.UpdateManyAsync(items);
            return Ok(new { updatedCount });
        }

        // DELETE: api/riskassessmentinherentriskS/bulk
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteMany([FromBody] List<string> ids)
        {
            var deletedCount = await _service.DeleteManyAsync(ids);
            return Ok(new { deletedCount });
        }

        // ===== Update by No (key) =====

        // SINGLE UPDATE BY No
        // PUT: api/riskassessmentinherentriskS/by-no/5.1
        [HttpPut("by-no/{no}")]
        public async Task<IActionResult> UpdateByNo(double no, [FromBody] RiskAssessmentInherentRisk body)
        {
            var success = await _service.UpdateByNoAsync(no, body);

            if (!success) return NotFound();  // no record with that No
            return NoContent();
        }

        // BULK UPDATE BY No
        // PUT: api/riskassessmentinherentriskS/bulk-by-no
        [HttpPut("bulk-by-no")]
        public async Task<IActionResult> BulkUpdateByNo([FromBody] List<RiskAssessmentInherentRisk> items)
        {
            var updatedCount = await _service.BulkUpdateByNoAsync(items);
            return Ok(new { updatedCount });
        }
    }
}
