using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class RiskAssessmentResidualRiskService
    {
        private readonly IMongoCollection<RiskAssessmentResidualRisk> _risks;

        public RiskAssessmentResidualRiskService(IMongoDatabase database)
        {
            // collection name in Mongo: "Risk Assessment (Residual Risk)"
            _risks = database.GetCollection<RiskAssessmentResidualRisk>(
                "Risk Assessment (Residual Risk)"
            );
        }

        // ===== Single-item CRUD =====

public async Task<PagedResult<RiskAssessmentResidualRisk>> GetAllAsync(
    int page = 1,
    string? search = null,
    int pageSize = 10,
    bool sortByNoAsc = false)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // ----- Search filter -----
    var filter = Builders<RiskAssessmentResidualRisk>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<RiskAssessmentResidualRisk>.Filter.Or(
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.Process, regex),
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.RiskType, regex),
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.RiskDescription, regex),
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.SeverityImpact, regex),
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.ProbabilityLikelihood, regex),
            Builders<RiskAssessmentResidualRisk>.Filter.Regex(x => x.Classification, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _risks.CountDocumentsAsync(filter);

    // ----- Build sort definition -----
    IFindFluent<RiskAssessmentResidualRisk, RiskAssessmentResidualRisk> query =
        _risks.Find(filter);

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

    return new PagedResult<RiskAssessmentResidualRisk>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<RiskAssessmentResidualRisk?> GetByIdAsync(string id)
        {
            return await _risks.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RiskAssessmentResidualRisk> CreateAsync(RiskAssessmentResidualRisk item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _risks.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, RiskAssessmentResidualRisk updated)
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

        public async Task<List<RiskAssessmentResidualRisk>> CreateManyAsync(
            IEnumerable<RiskAssessmentResidualRisk> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<RiskAssessmentResidualRisk> items)
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

            var filter = Builders<RiskAssessmentResidualRisk>.Filter.In(r => r.Id!, idList);
            var result = await _risks.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, RiskAssessmentResidualRisk updated)
        {
            var filter = Builders<RiskAssessmentResidualRisk>.Filter.Eq(r => r.No, no);

            var update = Builders<RiskAssessmentResidualRisk>.Update
                .Set(r => r.Process,               updated.Process)
                .Set(r => r.RiskType,              updated.RiskType)
                .Set(r => r.RiskDescription,       updated.RiskDescription)
                .Set(r => r.SeverityImpact,        updated.SeverityImpact)
                .Set(r => r.ProbabilityLikelihood, updated.ProbabilityLikelihood)
                .Set(r => r.Classification,        updated.Classification);

            var result = await _risks.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<RiskAssessmentResidualRisk> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<RiskAssessmentResidualRisk>.Filter.Eq(r => r.No, item.No);

                var update = Builders<RiskAssessmentResidualRisk>.Update
                    .Set(r => r.Process,               item.Process)
                    .Set(r => r.RiskType,              item.RiskType)
                    .Set(r => r.RiskDescription,       item.RiskDescription)
                    .Set(r => r.SeverityImpact,        item.SeverityImpact)
                    .Set(r => r.ProbabilityLikelihood, item.ProbabilityLikelihood)
                    .Set(r => r.Classification,        item.Classification);

                var result = await _risks.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
