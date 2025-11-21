using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class FinancialStatementAssertionService
    {
        private readonly IMongoCollection<FinancialStatementAssertion> _assertions;

        public FinancialStatementAssertionService(IMongoDatabase database)
        {
            // collection name in Mongo: "Financial Statement Assertions"
            _assertions = database.GetCollection<FinancialStatementAssertion>(
                "Financial Statement Assertions"
            );
        }

        // ===== Single-item CRUD =====
public async Task<PagedResult<FinancialStatementAssertion>> GetAllAsync(int page = 1, string? search = null)
{
    const int PageSize = 10;
    if (page < 1) page = 1;

    // ----- Search filter -----
    var filter = Builders<FinancialStatementAssertion>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<FinancialStatementAssertion>.Filter.Or(
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Process, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.InternalControlOverFinancialReporting, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Occurrence, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Completeness, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Accuracy, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Authorization, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Cutoff, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.ClassificationAndUnderstandability, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.Existence, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.RightsAndObligations, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.ValuationAndAllocation, regex),
            Builders<FinancialStatementAssertion>.Filter.Regex(x => x.PresentationDisclosure, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _assertions.CountDocumentsAsync(filter);

    // ----- Query page, latest on top -----
    var items = await _assertions
        .Find(filter)
        .SortByDescending(x => x.Date)   // latest first
        .ThenByDescending(x => x.No)     // tie-breaker
        .Skip((page - 1) * PageSize)
        .Limit(PageSize)
        .ToListAsync();

    var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

    return new PagedResult<FinancialStatementAssertion>
    {
        Page = page,
        PageSize = PageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}

        public async Task<FinancialStatementAssertion?> GetByIdAsync(string id)
        {
            return await _assertions.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<FinancialStatementAssertion> CreateAsync(FinancialStatementAssertion item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _assertions.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, FinancialStatementAssertion updated)
        {
            updated.Id = id;
            var result = await _assertions.ReplaceOneAsync(a => a.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _assertions.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<FinancialStatementAssertion>> CreateManyAsync(
            IEnumerable<FinancialStatementAssertion> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var a in list)
            {
                a.Id = null;

                  if (a.Date == default) // if not provided, set it
                            a.Date = DateTime.UtcNow;
            }

            await _assertions.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<FinancialStatementAssertion> items)
        {
            long modified = 0;

            foreach (var a in items)
            {
                if (string.IsNullOrWhiteSpace(a.Id))
                    continue;

                var result = await _assertions.ReplaceOneAsync(
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

            var filter = Builders<FinancialStatementAssertion>.Filter.In(a => a.Id!, idList);
            var result = await _assertions.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, FinancialStatementAssertion updated)
        {
            var filter = Builders<FinancialStatementAssertion>.Filter.Eq(a => a.No, no);

            var update = Builders<FinancialStatementAssertion>.Update
                .Set(a => a.Process,                           updated.Process)
                .Set(a => a.InternalControlOverFinancialReporting, updated.InternalControlOverFinancialReporting)
                .Set(a => a.Occurrence,                        updated.Occurrence)
                .Set(a => a.Completeness,                      updated.Completeness)
                .Set(a => a.Accuracy,                          updated.Accuracy)
                .Set(a => a.Authorization,                     updated.Authorization)
                .Set(a => a.Cutoff,                            updated.Cutoff)
                .Set(a => a.ClassificationAndUnderstandability,updated.ClassificationAndUnderstandability)
                .Set(a => a.Existence,                         updated.Existence)
                .Set(a => a.RightsAndObligations,              updated.RightsAndObligations)
                .Set(a => a.ValuationAndAllocation,            updated.ValuationAndAllocation)
                .Set(a => a.PresentationDisclosure,            updated.PresentationDisclosure);

            var result = await _assertions.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<FinancialStatementAssertion> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<FinancialStatementAssertion>.Filter.Eq(a => a.No, item.No);

                var update = Builders<FinancialStatementAssertion>.Update
                    .Set(a => a.Process,                           item.Process)
                    .Set(a => a.InternalControlOverFinancialReporting, item.InternalControlOverFinancialReporting)
                    .Set(a => a.Occurrence,                        item.Occurrence)
                    .Set(a => a.Completeness,                      item.Completeness)
                    .Set(a => a.Accuracy,                          item.Accuracy)
                    .Set(a => a.Authorization,                     item.Authorization)
                    .Set(a => a.Cutoff,                            item.Cutoff)
                    .Set(a => a.ClassificationAndUnderstandability,item.ClassificationAndUnderstandability)
                    .Set(a => a.Existence,                         item.Existence)
                    .Set(a => a.RightsAndObligations,              item.RightsAndObligations)
                    .Set(a => a.ValuationAndAllocation,            item.ValuationAndAllocation)
                    .Set(a => a.PresentationDisclosure,            item.PresentationDisclosure);

                var result = await _assertions.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
