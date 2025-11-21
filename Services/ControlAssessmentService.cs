using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class ControlAssessmentService
    {
        private readonly IMongoCollection<ControlAssessment> _assessments;

        public ControlAssessmentService(IMongoDatabase database)
        {
            // collection name in Mongo: "Control Assessment"
            _assessments = database.GetCollection<ControlAssessment>("Control Assessment");
        }

        // ===== Single-item CRUD =====

    public async Task<PagedResult<ControlAssessment>> GetAllAsync(int page = 1, string? search = null)
{
    const int PageSize = 10;
    if (page < 1) page = 1;

    // ----- Search filter -----
    var filter = Builders<ControlAssessment>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<ControlAssessment>.Filter.Or(
            Builders<ControlAssessment>.Filter.Regex(x => x.Process, regex),
            Builders<ControlAssessment>.Filter.Regex(x => x.LevelOfResponsibilityOperatingLevel, regex),
            Builders<ControlAssessment>.Filter.Regex(x => x.CosoPrincipleNumber, regex),
            Builders<ControlAssessment>.Filter.Regex(x => x.OperationalApproach, regex),
            Builders<ControlAssessment>.Filter.Regex(x => x.OperationalFrequency, regex),
            Builders<ControlAssessment>.Filter.Regex(x => x.ControlClassification, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _assessments.CountDocumentsAsync(filter);

    // ----- Query page, latest on top -----
    var items = await _assessments
        .Find(filter)
        .SortByDescending(x => x.Date)     // latest first
        .ThenByDescending(x => x.No)       // tie-breaker if same date
        .Skip((page - 1) * PageSize)
        .Limit(PageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

    return new PagedResult<ControlAssessment>
    {
        Page = page,
        PageSize = PageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<ControlAssessment?> GetByIdAsync(string id)
        {
            return await _assessments.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ControlAssessment> CreateAsync(ControlAssessment item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _assessments.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, ControlAssessment updated)
        {
            updated.Id = id;
            var result = await _assessments.ReplaceOneAsync(a => a.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _assessments.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<ControlAssessment>> CreateManyAsync(IEnumerable<ControlAssessment> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var a in list)
            {
                a.Id = null;

                  if (a.Date == default) // if not provided, set it
                            a.Date = DateTime.UtcNow;
            }

            await _assessments.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<ControlAssessment> items)
        {
            long modified = 0;

            foreach (var a in items)
            {
                if (string.IsNullOrWhiteSpace(a.Id))
                    continue;

                var result = await _assessments.ReplaceOneAsync(
                    x => x.Id == a.Id,
                    a
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<ControlAssessment>.Filter.In(a => a.Id!, idList);
            var result = await _assessments.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, ControlAssessment updated)
        {
            var filter = Builders<ControlAssessment>.Filter.Eq(a => a.No, no);

            var update = Builders<ControlAssessment>.Update
                .Set(a => a.Process,                          updated.Process)
                .Set(a => a.LevelOfResponsibilityOperatingLevel, updated.LevelOfResponsibilityOperatingLevel)
                .Set(a => a.CosoPrincipleNumber,              updated.CosoPrincipleNumber)
                .Set(a => a.OperationalApproach,              updated.OperationalApproach)
                .Set(a => a.OperationalFrequency,             updated.OperationalFrequency)
                .Set(a => a.ControlClassification,            updated.ControlClassification);

            var result = await _assessments.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<ControlAssessment> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<ControlAssessment>.Filter.Eq(a => a.No, item.No);

                var update = Builders<ControlAssessment>.Update
                    .Set(a => a.Process,                          item.Process)
                    .Set(a => a.LevelOfResponsibilityOperatingLevel, item.LevelOfResponsibilityOperatingLevel)
                    .Set(a => a.CosoPrincipleNumber,              item.CosoPrincipleNumber)
                    .Set(a => a.OperationalApproach,              item.OperationalApproach)
                    .Set(a => a.OperationalFrequency,             item.OperationalFrequency)
                    .Set(a => a.ControlClassification,            item.ControlClassification);

                var result = await _assessments.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
