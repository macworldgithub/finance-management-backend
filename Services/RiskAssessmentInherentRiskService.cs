using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class RiskAssessmentInherentRiskService
    {
        private readonly IMongoCollection<RiskAssessmentInherentRisk> _risks;

        public RiskAssessmentInherentRiskService(IMongoDatabase database)
        {
            // collection name in Mongo: "Risk Assessment  (Inherent Risk)"
            _risks = database.GetCollection<RiskAssessmentInherentRisk>(
                "Risk Assessment  (Inherent Risk)"
            );
        }

        // ===== Single-item CRUD =====

    public async Task<PagedResult<RiskAssessmentInherentRisk>> GetAllAsync(int page = 1, string? search = null)
{
    const int PageSize = 10;
    if (page < 1) page = 1;

    // ----- Search filter -----
    var filter = Builders<RiskAssessmentInherentRisk>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<RiskAssessmentInherentRisk>.Filter.Or(
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.Process, regex),
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.RiskType, regex),
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.RiskDescription, regex),
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.SeverityImpact, regex),
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.ProbabilityLikelihood, regex),
            Builders<RiskAssessmentInherentRisk>.Filter.Regex(x => x.Classification, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _risks.CountDocumentsAsync(filter);

    // ----- Query page, latest on top -----
    var items = await _risks
        .Find(filter)
        .SortByDescending(x => x.Date)   // newest first
        .ThenByDescending(x => x.No)     // tie-breaker
        .Skip((page - 1) * PageSize)
        .Limit(PageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

    return new PagedResult<RiskAssessmentInherentRisk>
    {
        Page = page,
        PageSize = PageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<RiskAssessmentInherentRisk?> GetByIdAsync(string id)
        {
            return await _risks.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RiskAssessmentInherentRisk> CreateAsync(RiskAssessmentInherentRisk item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _risks.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, RiskAssessmentInherentRisk updated)
        {
            updated.Id = id;
            var result = await _risks.ReplaceOneAsync(r => r.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _risks.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<RiskAssessmentInherentRisk>> CreateManyAsync(IEnumerable<RiskAssessmentInherentRisk> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var r in list)
            {
                r.Id = null;

                  if (r.Date == default) // if not provided, set it
                            r.Date = DateTime.UtcNow;
            }

            await _risks.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<RiskAssessmentInherentRisk> items)
        {
            long modified = 0;

            foreach (var r in items)
            {
                if (string.IsNullOrWhiteSpace(r.Id))
                    continue;

                var result = await _risks.ReplaceOneAsync(
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

            var filter = Builders<RiskAssessmentInherentRisk>.Filter.In(r => r.Id!, idList);
            var result = await _risks.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, RiskAssessmentInherentRisk updated)
        {
            var filter = Builders<RiskAssessmentInherentRisk>.Filter.Eq(r => r.No, no);

            var update = Builders<RiskAssessmentInherentRisk>.Update
                .Set(r => r.Process,              updated.Process)
                .Set(r => r.RiskType,             updated.RiskType)
                .Set(r => r.RiskDescription,      updated.RiskDescription)
                .Set(r => r.SeverityImpact,       updated.SeverityImpact)
                .Set(r => r.ProbabilityLikelihood,updated.ProbabilityLikelihood)
                .Set(r => r.Classification,       updated.Classification);

            var result = await _risks.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<RiskAssessmentInherentRisk> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<RiskAssessmentInherentRisk>.Filter.Eq(r => r.No, item.No);

                var update = Builders<RiskAssessmentInherentRisk>.Update
                    .Set(r => r.Process,              item.Process)
                    .Set(r => r.RiskType,             item.RiskType)
                    .Set(r => r.RiskDescription,      item.RiskDescription)
                    .Set(r => r.SeverityImpact,       item.SeverityImpact)
                    .Set(r => r.ProbabilityLikelihood,item.ProbabilityLikelihood)
                    .Set(r => r.Classification,       item.Classification);

                var result = await _risks.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
