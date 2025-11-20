using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class TransactionService
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionService(IMongoDatabase database)
        {
            // Collection name in MongoDB = "Transactions"
            _transactions = database.GetCollection<Transaction>("Transactions");
        }

        // CREATE
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await _transactions.InsertOneAsync(transaction);
            return transaction;
        }

        // READ ALL
        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _transactions.Find(_ => true).ToListAsync();
        }

        // READ ONE
        public async Task<Transaction?> GetByIdAsync(string id)
        {
            return await _transactions.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        // UPDATE
        public async Task<bool> UpdateAsync(string id, Transaction updated)
        {
            updated.Id = id;
            var result = await _transactions.ReplaceOneAsync(t => t.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _transactions.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
