using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson; 

namespace finance_management_backend.Services
{
    public class GrcExceptionLogService
    {
        private readonly IMongoCollection<GrcExceptionLog> _logs;

        public GrcExceptionLogService(IMongoDatabase database)
        {
            // collection name in Mongo: "GRC Exception Log"
            _logs = database.GetCollection<GrcExceptionLog>("GRC Exception Log");
        }

        // ===== Single-item CRUD =====

public async Task<PagedResult<GrcExceptionLog>> GetAllAsync(
    int page = 1,
    string? search = null,
    int pageSize = 10,
    bool sortByNoAsc = false)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // ----- Search filter -----
    var filter = Builders<GrcExceptionLog>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<GrcExceptionLog>.Filter.Or(
            Builders<GrcExceptionLog>.Filter.Regex(x => x.Process, regex),
            Builders<GrcExceptionLog>.Filter.Regex(x => x.GrcAdequacy, regex),
            Builders<GrcExceptionLog>.Filter.Regex(x => x.GrcEffectiveness, regex),
            Builders<GrcExceptionLog>.Filter.Regex(x => x.Explanation, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _logs.CountDocumentsAsync(filter);

    // ----- Build sort definition -----
    IFindFluent<GrcExceptionLog, GrcExceptionLog> query = _logs.Find(filter);

    if (sortByNoAsc)
    {
        // sort by No ascending
        query = query.SortBy(x => x.No);
    }
    else
    {
        // default: latest Date first, then No desc
        query = query
            .SortByDescending(x => x.Date)
            .ThenByDescending(x => x.No);
    }

    // ----- Query page -----
    var items = await query
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    return new PagedResult<GrcExceptionLog>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<GrcExceptionLog?> GetByIdAsync(string id)
        {
            return await _logs.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<GrcExceptionLog> CreateAsync(GrcExceptionLog item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _logs.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, GrcExceptionLog updated)
        {
            updated.Id = id;
            var result = await _logs.ReplaceOneAsync(x => x.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _logs.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once =====

        public async Task<List<GrcExceptionLog>> CreateManyAsync(IEnumerable<GrcExceptionLog> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var x in list)
            {
                x.Id = null;

                  if (x.Date == default) // if not provided, set it
                            x.Date = DateTime.UtcNow;
            }

            await _logs.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<GrcExceptionLog> items)
        {
            long modified = 0;

            foreach (var x in items)
            {
                if (string.IsNullOrWhiteSpace(x.Id))
                    continue;

                var result = await _logs.ReplaceOneAsync(
                    y => y.Id == x.Id,
                    x
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<GrcExceptionLog>.Filter.In(x => x.Id!, idList);
            var result = await _logs.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – everything except No =====

        public async Task<bool> UpdateByNoAsync(double no, GrcExceptionLog updated)
        {
            var filter = Builders<GrcExceptionLog>.Filter.Eq(x => x.No, no);

            var update = Builders<GrcExceptionLog>.Update
                .Set(x => x.Process,        updated.Process)
                .Set(x => x.GrcAdequacy,    updated.GrcAdequacy)
                .Set(x => x.GrcEffectiveness, updated.GrcEffectiveness)
                .Set(x => x.Explanation,    updated.Explanation);

            var result = await _logs.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(IEnumerable<GrcExceptionLog> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<GrcExceptionLog>.Filter.Eq(x => x.No, item.No);

                var update = Builders<GrcExceptionLog>.Update
                    .Set(x => x.Process,          item.Process)
                    .Set(x => x.GrcAdequacy,      item.GrcAdequacy)
                    .Set(x => x.GrcEffectiveness, item.GrcEffectiveness)
                    .Set(x => x.Explanation,      item.Explanation);

                var result = await _logs.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
