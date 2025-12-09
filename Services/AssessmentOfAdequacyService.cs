// Services/AssessmentOfAdequacyService.cs
using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;                    // ← THIS LINE WAS MISSING
namespace finance_management_backend.Services
{
    public class AssessmentOfAdequacyService
    {
        private readonly IMongoCollection<AssessmentOfAdequacy> _collection;

        public AssessmentOfAdequacyService(IMongoDatabase database)
        {
            _collection = database.GetCollection<AssessmentOfAdequacy>("AssessmentOfAdequacy");
        }

        public async Task<PagedResult<AssessmentOfAdequacy>> GetAllAsync(int page = 1, string? search = null, int pageSize = 10)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    var filter = Builders<AssessmentOfAdequacy>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        search = search.Trim();
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<AssessmentOfAdequacy>.Filter.Or(
            Builders<AssessmentOfAdequacy>.Filter.Regex(x => x.Process, regex),
            Builders<AssessmentOfAdequacy>.Filter.Regex(x => x.No, new BsonRegularExpression($"^{search}", "i")), // starts with number
            Builders<AssessmentOfAdequacy>.Filter.Regex(x => x.Rating, regex) // now safely searches string Rating
        );
    }

    var totalItems = await _collection.CountDocumentsAsync(filter);

    var items = await _collection.Find(filter)
        .SortByDescending(x => x.Date)
        .Skip((page - 1) * pageSize)
        .Limit(pageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

    return new PagedResult<AssessmentOfAdequacy>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}

        public async Task<AssessmentOfAdequacy?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<AssessmentOfAdequacy> CreateAsync(AssessmentOfAdequacy item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, AssessmentOfAdequacy item)
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

        public async Task<List<AssessmentOfAdequacy>> CreateManyAsync(IEnumerable<AssessmentOfAdequacy> items)
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