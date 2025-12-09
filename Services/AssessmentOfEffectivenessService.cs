// Services/AssessmentOfEffectivenessService.cs
using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;                    // ← THIS LINE WAS MISSING
namespace finance_management_backend.Services
{
    public class AssessmentOfEffectivenessService
    {
        private readonly IMongoCollection<AssessmentOfEffectiveness> _collection;

        public AssessmentOfEffectivenessService(IMongoDatabase database)
        {
            _collection = database.GetCollection<AssessmentOfEffectiveness>("AssessmentOfEffectiveness");
        }

        // Same methods as above – just change the generic type
        public async Task<PagedResult<AssessmentOfEffectiveness>> GetAllAsync(int page = 1, string? search = null, int pageSize = 10)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    var filter = Builders<AssessmentOfEffectiveness>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();
        var regex = new BsonRegularExpression(search, "i");

        filter = Builders<AssessmentOfEffectiveness>.Filter.Or(
            Builders<AssessmentOfEffectiveness>.Filter.Regex(x => x.Process, regex),
            Builders<AssessmentOfEffectiveness>.Filter.Regex(x => x.No, new BsonRegularExpression($"^{search}", "i")),
            Builders<AssessmentOfEffectiveness>.Filter.Regex(x => x.Rating, regex) // string search
        );
    }

    var totalItems = await _collection.CountDocumentsAsync(filter);

    var items = await _collection.Find(filter)
        .SortByDescending(x => x.Date)
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    return new PagedResult<AssessmentOfEffectiveness>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}

        public async Task<AssessmentOfEffectiveness?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<AssessmentOfEffectiveness> CreateAsync(AssessmentOfEffectiveness item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, AssessmentOfEffectiveness item)
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

        public async Task<List<AssessmentOfEffectiveness>> CreateManyAsync(IEnumerable<AssessmentOfEffectiveness> items)
        {
            var list = items.ToList();
            foreach (var i in list)
            {
                i.Id = null;
                if (i.Date == default) i.Date = DateTime.UtcNow;
            }
            await _collection.InsertManyAsync(list);
            return list;
        }
    }
}