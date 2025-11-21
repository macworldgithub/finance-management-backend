using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson; 

namespace finance_management_backend.Services
{
    public class ControlActivityService
    {
        private readonly IMongoCollection<ControlActivity> _controls;

        public ControlActivityService(IMongoDatabase database)
        {
            // collection name in Mongo: "Control Activities"
            _controls = database.GetCollection<ControlActivity>("Control Activities");
        }

        // ===== Single-item CRUD =====

  public async Task<PagedResult<ControlActivity>> GetAllAsync(int page = 1, string? search = null)
{
    const int PageSize = 10;
    if (page < 1) page = 1;

    // ----- Search filter -----
    var filter = Builders<ControlActivity>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<ControlActivity>.Filter.Or(
            Builders<ControlActivity>.Filter.Regex(x => x.Process, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ControlObjectives, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ControlRef, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ControlDefinition, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ControlDescription, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ControlResponsibility, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.KeyControl, regex),
            Builders<ControlActivity>.Filter.Regex(x => x.ZeroTolerance, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _controls.CountDocumentsAsync(filter);

    // ----- Query page, latest on top -----
    var items = await _controls
        .Find(filter)
        .SortByDescending(x => x.Date)          // latest first
        .ThenByDescending(x => x.No)           // tie-breaker
        .Skip((page - 1) * PageSize)
        .Limit(PageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

    return new PagedResult<ControlActivity>
    {
        Page = page,
        PageSize = PageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<ControlActivity?> GetByIdAsync(string id)
        {
            return await _controls.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ControlActivity> CreateAsync(ControlActivity item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _controls.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, ControlActivity updated)
        {
            updated.Id = id;
            var result = await _controls.ReplaceOneAsync(c => c.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _controls.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<ControlActivity>> CreateManyAsync(IEnumerable<ControlActivity> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var c in list)
            {
                c.Id = null;

                        if (c.Date == default) // if not provided, set it
                            c.Date = DateTime.UtcNow;
            }

            await _controls.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<ControlActivity> items)
        {
            long modified = 0;

            foreach (var c in items)
            {
                if (string.IsNullOrWhiteSpace(c.Id))
                    continue;

                var result = await _controls.ReplaceOneAsync(
                    x => x.Id == c.Id,
                    c
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<ControlActivity>.Filter.In(c => c.Id!, idList);
            var result = await _controls.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, ControlActivity updated)
        {
            var filter = Builders<ControlActivity>.Filter.Eq(c => c.No, no);

            var update = Builders<ControlActivity>.Update
                .Set(c => c.Process,              updated.Process)
                .Set(c => c.ControlObjectives,    updated.ControlObjectives)
                .Set(c => c.ControlRef,           updated.ControlRef)
                .Set(c => c.ControlDefinition,    updated.ControlDefinition)
                .Set(c => c.ControlDescription,   updated.ControlDescription)
                .Set(c => c.ControlResponsibility,updated.ControlResponsibility)
                .Set(c => c.KeyControl,           updated.KeyControl)
                .Set(c => c.ZeroTolerance,        updated.ZeroTolerance);

            var result = await _controls.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<ControlActivity> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<ControlActivity>.Filter.Eq(c => c.No, item.No);

                var update = Builders<ControlActivity>.Update
                    .Set(c => c.Process,              item.Process)
                    .Set(c => c.ControlObjectives,    item.ControlObjectives)
                    .Set(c => c.ControlRef,           item.ControlRef)
                    .Set(c => c.ControlDefinition,    item.ControlDefinition)
                    .Set(c => c.ControlDescription,   item.ControlDescription)
                    .Set(c => c.ControlResponsibility,item.ControlResponsibility)
                    .Set(c => c.KeyControl,           item.KeyControl)
                    .Set(c => c.ZeroTolerance,        item.ZeroTolerance);

                var result = await _controls.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
