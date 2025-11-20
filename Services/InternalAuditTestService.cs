using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class InternalAuditTestService
    {
        private readonly IMongoCollection<InternalAuditTest> _tests;

        public InternalAuditTestService(IMongoDatabase database)
        {
            // collection name in Mongo: "Internal Audit Test"
            _tests = database.GetCollection<InternalAuditTest>("Internal Audit Test");
        }

        // ===== Single-item CRUD =====

        public async Task<List<InternalAuditTest>> GetAllAsync()
        {
            return await _tests.Find(_ => true).ToListAsync();
        }

        public async Task<InternalAuditTest?> GetByIdAsync(string id)
        {
            return await _tests.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<InternalAuditTest> CreateAsync(InternalAuditTest item)
        {
            item.Id = null; // let Mongo generate Id
            await _tests.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, InternalAuditTest updated)
        {
            updated.Id = id;
            var result = await _tests.ReplaceOneAsync(t => t.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _tests.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<InternalAuditTest>> CreateManyAsync(IEnumerable<InternalAuditTest> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var t in list)
            {
                t.Id = null;
            }

            await _tests.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<InternalAuditTest> items)
        {
            long modified = 0;

            foreach (var t in items)
            {
                if (string.IsNullOrWhiteSpace(t.Id))
                    continue;

                var result = await _tests.ReplaceOneAsync(
                    x => x.Id == t.Id,
                    t
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<InternalAuditTest>.Filter.In(t => t.Id!, idList);
            var result = await _tests.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, InternalAuditTest updated)
        {
            var filter = Builders<InternalAuditTest>.Filter.Eq(t => t.No, no);

            var update = Builders<InternalAuditTest>.Update
                .Set(t => t.Process,             updated.Process)
                .Set(t => t.Check,               updated.Check)
                .Set(t => t.InternalAuditTestName, updated.InternalAuditTestName)
                .Set(t => t.SampleSize,          updated.SampleSize);

            var result = await _tests.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<InternalAuditTest> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<InternalAuditTest>.Filter.Eq(t => t.No, item.No);

                var update = Builders<InternalAuditTest>.Update
                    .Set(t => t.Process,             item.Process)
                    .Set(t => t.Check,               item.Check)
                    .Set(t => t.InternalAuditTestName, item.InternalAuditTestName)
                    .Set(t => t.SampleSize,          item.SampleSize);

                var result = await _tests.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
