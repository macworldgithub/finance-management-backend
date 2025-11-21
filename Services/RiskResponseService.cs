using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class RiskResponseService
    {
        private readonly IMongoCollection<RiskResponse> _riskResponses;

        public RiskResponseService(IMongoDatabase database)
        {
            // collection name in Mongo: "Risk Responses"
            _riskResponses = database.GetCollection<RiskResponse>("Risk Responses");
        }

        // ===== Single-item CRUD =====

        public async Task<List<RiskResponse>> GetAllAsync()
        {
            return await _riskResponses.Find(_ => true).ToListAsync();
        }

        public async Task<RiskResponse?> GetByIdAsync(string id)
        {
            return await _riskResponses.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RiskResponse> CreateAsync(RiskResponse item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _riskResponses.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, RiskResponse updated)
        {
            updated.Id = id;
            var result = await _riskResponses.ReplaceOneAsync(r => r.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _riskResponses.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<RiskResponse>> CreateManyAsync(IEnumerable<RiskResponse> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var r in list)
            {
                r.Id = null;
                  if (r.Date == default) // if not provided, set it
                            r.Date = DateTime.UtcNow;
            }

            await _riskResponses.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<RiskResponse> items)
        {
            long modified = 0;

            foreach (var r in items)
            {
                if (string.IsNullOrWhiteSpace(r.Id))
                    continue;

                var result = await _riskResponses.ReplaceOneAsync(
                    x => x.Id == r.Id,
                    r
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<RiskResponse>.Filter.In(r => r.Id!, idList);
            var result = await _riskResponses.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, RiskResponse updated)
        {
            var filter = Builders<RiskResponse>.Filter.Eq(r => r.No, no);

            var update = Builders<RiskResponse>.Update
                .Set(r => r.Process,           updated.Process)
                .Set(r => r.TypeOfRiskResponse, updated.TypeOfRiskResponse);

            var result = await _riskResponses.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<RiskResponse> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<RiskResponse>.Filter.Eq(r => r.No, item.No);

                var update = Builders<RiskResponse>.Update
                    .Set(r => r.Process,           item.Process)
                    .Set(r => r.TypeOfRiskResponse, item.TypeOfRiskResponse);

                var result = await _riskResponses.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
