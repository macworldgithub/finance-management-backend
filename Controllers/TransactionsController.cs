using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Models;
using finance_management_backend.Services;

namespace finance_management_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] 
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll()
        {
            var items = await _transactionService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById(string id)
        {
            var item = await _transactionService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> Create(Transaction transaction)
        {
            var created = await _transactionService.CreateAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Transaction updated)
        {
            var existing = await _transactionService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _transactionService.UpdateAsync(id, updated);
            if (!success) return StatusCode(500, "Failed to update");

            return NoContent();
        }

        // DELETE: api/transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _transactionService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var success = await _transactionService.DeleteAsync(id);
            if (!success) return StatusCode(500, "Failed to delete");

            return NoContent();
        }
    }
}
