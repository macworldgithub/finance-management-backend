using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class ProcessService
    {
        private readonly IMongoCollection<Process> _processes;

        public ProcessService(IMongoDatabase database)
        {
            // collection name in Mongo: "Process"
            _processes = database.GetCollection<Process>("Process");
        }

        // ===== Single-item CRUD =====

     public async Task<PagedResult<Process>> GetAllAsync(int page = 1, string? search = null)
{
    const int PageSize = 10;
    if (page < 1) page = 1;

    // ----- Search filter -----
    var filter = Builders<Process>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<Process>.Filter.Or(
            Builders<Process>.Filter.Regex(x => x.ProcessName, regex),
            Builders<Process>.Filter.Regex(x => x.ProcessDescription, regex),
            Builders<Process>.Filter.Regex(x => x.ProcessObjectives, regex),
            Builders<Process>.Filter.Regex(x => x.ProcessSeverityLevels, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _processes.CountDocumentsAsync(filter);

    // ----- Query page, latest on top -----
    var items = await _processes
        .Find(filter)
        .SortByDescending(x => x.Date)   // newest first
        .ThenByDescending(x => x.No)     // tie-breaker
        .Skip((page - 1) * PageSize)
        .Limit(PageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

    return new PagedResult<Process>
    {
        Page = page,
        PageSize = PageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<Process?> GetByIdAsync(string id)
        {
            return await _processes.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Process> CreateAsync(Process process)
        {
            // let Mongo generate Id if null
            process.Id = null;
            process.Date = DateTime.UtcNow; // NEW: set current date/time
            await _processes.InsertOneAsync(process);
            return process;
        }

        public async Task<bool> UpdateAsync(string id, Process updated)
        {
            updated.Id = id;
            var result = await _processes.ReplaceOneAsync(p => p.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _processes.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        // Create many
        public async Task<List<Process>> CreateManyAsync(IEnumerable<Process> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var p in list)
            {
                p.Id = null; // force Mongo to generate ids

                  if (p.Date == default) // if not provided, set it
                            p.Date = DateTime.UtcNow;
            }

            await _processes.InsertManyAsync(list);
            return list;
        }

        // Update many (by Id)
        public async Task<long> UpdateManyAsync(IEnumerable<Process> items)
        {
            long modified = 0;

            foreach (var p in items)
            {
                if (string.IsNullOrWhiteSpace(p.Id))
                    continue;

                var result = await _processes.ReplaceOneAsync(
                    x => x.Id == p.Id,
                    p
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        // Delete many (by list of Ids)
        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<Process>.Filter.In(p => p.Id!, idList);
            var result = await _processes.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, Process updated)
        {
            var filter = Builders<Process>.Filter.Eq(p => p.No, no);

            // we DO NOT set No in the update, so No stays the same
            var update = Builders<Process>.Update
                .Set(p => p.ProcessName,           updated.ProcessName)
                .Set(p => p.ProcessDescription,    updated.ProcessDescription)
                .Set(p => p.ProcessObjectives,     updated.ProcessObjectives)
                .Set(p => p.ProcessSeverityLevels, updated.ProcessSeverityLevels);

            var result = await _processes.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<Process> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<Process>.Filter.Eq(p => p.No, item.No);

                // again, No is NOT changed, only other fields
                var update = Builders<Process>.Update
                    .Set(p => p.ProcessName,           item.ProcessName)
                    .Set(p => p.ProcessDescription,    item.ProcessDescription)
                    .Set(p => p.ProcessObjectives,     item.ProcessObjectives)
                    .Set(p => p.ProcessSeverityLevels, item.ProcessSeverityLevels);

                var result = await _processes.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    
    
    }
}
