using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class OwnershipService
    {
        private readonly IMongoCollection<Ownership> _ownerships;

        public OwnershipService(IMongoDatabase database)
        {
            // collection name in Mongo: "Ownership"
            _ownerships = database.GetCollection<Ownership>("Ownership");
        }

        // ===== Single-item CRUD =====

        public async Task<List<Ownership>> GetAllAsync()
        {
            return await _ownerships.Find(_ => true).ToListAsync();
        }

        public async Task<Ownership?> GetByIdAsync(string id)
        {
            return await _ownerships.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Ownership> CreateAsync(Ownership ownership)
        {
            ownership.Id = null; // let Mongo generate Id
            await _ownerships.InsertOneAsync(ownership);
            return ownership;
        }

        public async Task<bool> UpdateAsync(string id, Ownership updated)
        {
            updated.Id = id;
            var result = await _ownerships.ReplaceOneAsync(o => o.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _ownerships.DeleteOneAsync(o => o.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<Ownership>> CreateManyAsync(IEnumerable<Ownership> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var o in list)
            {
                o.Id = null;
            }

            await _ownerships.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<Ownership> items)
        {
            long modified = 0;

            foreach (var o in items)
            {
                if (string.IsNullOrWhiteSpace(o.Id))
                    continue;

                var result = await _ownerships.ReplaceOneAsync(
                    x => x.Id == o.Id,
                    o
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<Ownership>.Filter.In(o => o.Id!, idList);
            var result = await _ownerships.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, Ownership updated)
        {
            var filter = Builders<Ownership>.Filter.Eq(o => o.No, no);

            var update = Builders<Ownership>.Update
                .Set(o => o.MainProcess,                          updated.MainProcess)
                .Set(o => o.Activity,                         updated.Activity)
                .Set(o => o.Process,                         updated.Process)
                .Set(o => o.ProcessStage,                     updated.ProcessStage)
                .Set(o => o.Functions,                        updated.Functions)
                .Set(o => o.ClientSegmentOrFunctionalSegment, updated.ClientSegmentOrFunctionalSegment)
                .Set(o => o.OperationalUnit,                  updated.OperationalUnit)
                .Set(o => o.Division,                         updated.Division)
                .Set(o => o.Entity,                           updated.Entity)
                .Set(o => o.UnitOrDepartment,                 updated.UnitOrDepartment)
                .Set(o => o.ProductClass,                     updated.ProductClass)
                .Set(o => o.ProductName,                      updated.ProductName);

            var result = await _ownerships.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<Ownership> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<Ownership>.Filter.Eq(o => o.No, item.No);

                var update = Builders<Ownership>.Update
                    .Set(o => o.MainProcess,                          item.MainProcess)
                    .Set(o => o.Activity,                         item.Activity)
                    .Set(o => o.Process,                         item.Process)
                    .Set(o => o.ProcessStage,                     item.ProcessStage)
                    .Set(o => o.Functions,                        item.Functions)
                    .Set(o => o.ClientSegmentOrFunctionalSegment, item.ClientSegmentOrFunctionalSegment)
                    .Set(o => o.OperationalUnit,                  item.OperationalUnit)
                    .Set(o => o.Division,                         item.Division)
                    .Set(o => o.Entity,                           item.Entity)
                    .Set(o => o.UnitOrDepartment,                 item.UnitOrDepartment)
                    .Set(o => o.ProductClass,                     item.ProductClass)
                    .Set(o => o.ProductName,                      item.ProductName);

                var result = await _ownerships.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
