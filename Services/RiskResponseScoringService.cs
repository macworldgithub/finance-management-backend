using MongoDB.Driver;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class RiskResponseScoringService
    {
        private readonly IMongoCollection<RiskResponseScoring> _collection;

        public RiskResponseScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<RiskResponseScoring>("RiskResponseScoring");
        }

        // Get paginated results with optional search
        public async Task<PagedResult<RiskResponseScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<RiskResponseScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new MongoDB.Bson.BsonRegularExpression(search, "i");
                filter = Builders<RiskResponseScoring>.Filter.Or(
                    Builders<RiskResponseScoring>.Filter.Regex(x => x.Process, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            var items = await _collection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            // Calculate row-wise total for each record
            items.ForEach(item =>
            {
                item.TotalScoreAvoid = item.Avoid;
                item.TotalScoreMitigate = item.Mitigate;
                item.TotalScoreTransfer = item.Transfer;
                item.TotalScoreShare = item.Share;
                item.TotalScoreAccept = item.Accept;
            });

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PagedResult<RiskResponseScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        // Get by Id
        public async Task<RiskResponseScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Create
        public async Task<RiskResponseScoring> CreateAsync(RiskResponseScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        // Update by Id
        public async Task<bool> UpdateAsync(string id, RiskResponseScoring item)
        {
            item.Id = id;
            var result = await _collection.ReplaceOneAsync(x => x.Id == id, item);
            return result.ModifiedCount > 0;
        }

        // Delete by Id
        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        // Column-wise totals
        public async Task<Dictionary<string, int>> GetColumnWiseSumsAsync()
        {
            var allItems = await _collection.Find(_ => true).ToListAsync();

            return new Dictionary<string, int>
            {
                ["TotalSumAvoid"] = allItems.Sum(x => x.Avoid),
                ["TotalSumMitigate"] = allItems.Sum(x => x.Mitigate),
                ["TotalSumTransfer"] = allItems.Sum(x => x.Transfer),
                ["TotalSumShare"] = allItems.Sum(x => x.Share),
                ["TotalSumAccept"] = allItems.Sum(x => x.Accept)
            };
        }
    }
}
