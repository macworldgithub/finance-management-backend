using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class RiskAssessmentInherentRiskScoringService
    {
        private readonly IMongoCollection<RiskAssessmentInherentRiskScoring> _collection;

        public RiskAssessmentInherentRiskScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<RiskAssessmentInherentRiskScoring>(
                "RiskAssessmentInherentRiskScoring");
        }

        public async Task<PagedResult<RiskAssessmentInherentRiskScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10,
            bool sortByNoAsc = false)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<RiskAssessmentInherentRiskScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new BsonRegularExpression(search, "i");
                filter = Builders<RiskAssessmentInherentRiskScoring>.Filter.Or(
                    Builders<RiskAssessmentInherentRiskScoring>.Filter.Regex(x => x.Process, regex),
                    Builders<RiskAssessmentInherentRiskScoring>.Filter.Regex(x => x.RiskId, regex),
                    Builders<RiskAssessmentInherentRiskScoring>.Filter.Regex(x => x.RiskType, regex),
                    Builders<RiskAssessmentInherentRiskScoring>.Filter.Regex(x => x.RiskDescription, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            var query = _collection.Find(filter);

            query = sortByNoAsc
                ? query.SortBy(x => x.No)
                : query.SortByDescending(x => x.Date).ThenByDescending(x => x.No);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PagedResult<RiskAssessmentInherentRiskScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = items
            };
        }

        public async Task<RiskAssessmentInherentRiskScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<RiskAssessmentInherentRiskScoring> CreateAsync(
            RiskAssessmentInherentRiskScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, RiskAssessmentInherentRiskScoring item)
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

        public async Task<List<RiskAssessmentInherentRiskScoring>> CreateManyAsync(
            IEnumerable<RiskAssessmentInherentRiskScoring> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<RiskAssessmentInherentRiskScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Id)) continue;
                var result = await _collection.ReplaceOneAsync(
                    x => x.Id == item.Id, item);
                modified += result.ModifiedCount;
            }
            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var list = ids.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (list.Count == 0) return 0;

            var filter = Builders<RiskAssessmentInherentRiskScoring>
                .Filter.In(x => x.Id!, list);

            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public async Task<bool> UpdateByNoAsync(
            double no,
            RiskAssessmentInherentRiskScoring updated)
        {
            var filter = Builders<RiskAssessmentInherentRiskScoring>
                .Filter.Eq(x => x.No, no);

            var result = await _collection.ReplaceOneAsync(filter, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(
            IEnumerable<RiskAssessmentInherentRiskScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                var filter = Builders<RiskAssessmentInherentRiskScoring>
                    .Filter.Eq(x => x.No, item.No);

                var result = await _collection.ReplaceOneAsync(filter, item);
                modified += result.ModifiedCount;
            }
            return modified;
        }
    }
}
