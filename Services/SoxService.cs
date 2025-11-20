using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class SoxService
    {
        private readonly IMongoCollection<Sox> _sox;

        public SoxService(IMongoDatabase database)
        {
            // collection name in Mongo: "SOX"
            _sox = database.GetCollection<Sox>("SOX");
        }

        // ===== Single-item CRUD =====

        public async Task<List<Sox>> GetAllAsync()
        {
            return await _sox.Find(_ => true).ToListAsync();
        }

        public async Task<Sox?> GetByIdAsync(string id)
        {
            return await _sox.Find(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Sox> CreateAsync(Sox item)
        {
            item.Id = null; // let Mongo generate Id
            await _sox.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, Sox updated)
        {
            updated.Id = id;
            var result = await _sox.ReplaceOneAsync(s => s.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _sox.DeleteOneAsync(s => s.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<Sox>> CreateManyAsync(IEnumerable<Sox> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var s in list)
            {
                s.Id = null;
            }

            await _sox.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<Sox> items)
        {
            long modified = 0;

            foreach (var s in items)
            {
                if (string.IsNullOrWhiteSpace(s.Id))
                    continue;

                var result = await _sox.ReplaceOneAsync(
                    x => x.Id == s.Id,
                    s
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<Sox>.Filter.In(s => s.Id!, idList);
            var result = await _sox.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, Sox updated)
        {
            var filter = Builders<Sox>.Filter.Eq(s => s.No, no);

            var update = Builders<Sox>.Update
                .Set(s => s.Process,           updated.Process)
                .Set(s => s.SoxControlActivity,updated.SoxControlActivity);

            var result = await _sox.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<Sox> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<Sox>.Filter.Eq(s => s.No, item.No);

                var update = Builders<Sox>.Update
                    .Set(s => s.Process,            item.Process)
                    .Set(s => s.SoxControlActivity, item.SoxControlActivity);

                var result = await _sox.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
