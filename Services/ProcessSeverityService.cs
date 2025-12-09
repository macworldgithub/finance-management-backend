using MongoDB.Bson;
using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class ProcessSeverityService
    {
        private readonly IMongoCollection<ProcessSeverity> _collection;

        public ProcessSeverityService(IMongoDatabase database)
        {
            _collection = database.GetCollection<ProcessSeverity>("ProcessSeverity");
        }

        public async Task<PagedResult<ProcessSeverity>> GetAllAsync(int page = 1, string? search = null, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<ProcessSeverity>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                var regex = new BsonRegularExpression(search, "i");

                filter = Builders<ProcessSeverity>.Filter.Or(
                    Builders<ProcessSeverity>.Filter.Regex(x => x.Process, regex),
                    Builders<ProcessSeverity>.Filter.Regex(x => x.No, new BsonRegularExpression($"^{search}", "i")),
                    Builders<ProcessSeverity>.Filter.Regex(x => x.Rating, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            var items = await _collection.Find(filter)
                .SortByDescending(x => x.Date)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PagedResult<ProcessSeverity>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<ProcessSeverity?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<ProcessSeverity> CreateAsync(ProcessSeverity item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, ProcessSeverity item)
        {
            item.Id = id;
            var result = await _collection.ReplaceOneAsync(x => x.Id == id, item);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<List<ProcessSeverity>> CreateManyAsync(IEnumerable<ProcessSeverity> items)
        {
            var list = items.ToList();
            foreach (var item in list)
            {
                item.Id = null;
                if (item.Date == default) item.Date = DateTime.UtcNow;
            }
            await _collection.InsertManyAsync(list);
            return list;
        }
    }
}