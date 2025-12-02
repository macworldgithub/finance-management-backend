using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class OtherControlEnvironmentService
    {
        private readonly IMongoCollection<OtherControlEnvironment> _other;

        public OtherControlEnvironmentService(IMongoDatabase database)
        {
            // collection name in Mongo: "Other- - Control Environment"
            _other = database.GetCollection<OtherControlEnvironment>("Other- - Control Environment");
        }

        // ===== Single-item CRUD =====

public async Task<PagedResult<OtherControlEnvironment>> GetAllAsync(
    int page = 1,
    string? search = null,
    int pageSize = 10,
    bool sortByNoAsc = false)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // ----- Search filter -----
    var filter = Builders<OtherControlEnvironment>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<OtherControlEnvironment>.Filter.Or(
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.Process, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.ResponsibilityDelegationMatrix, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.SegregationOfDuties, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.ReportingLines, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.Mission, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.VisionAndValues, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.GoalsAndObjectives, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.StructuresAndSystems, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.PoliciesAndProcedures, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.Processes, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.IntegrityAndEthicalValues, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.OversightStructure, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.Standards, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.Methodologies, regex),
            Builders<OtherControlEnvironment>.Filter.Regex(x => x.RulesAndRegulations, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _other.CountDocumentsAsync(filter);

    // ----- Build sort definition -----
    IFindFluent<OtherControlEnvironment, OtherControlEnvironment> query =
        _other.Find(filter);

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

    return new PagedResult<OtherControlEnvironment>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<OtherControlEnvironment?> GetByIdAsync(string id)
        {
            return await _other.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<OtherControlEnvironment> CreateAsync(OtherControlEnvironment item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _other.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, OtherControlEnvironment updated)
        {
            updated.Id = id;
            var result = await _other.ReplaceOneAsync(c => c.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _other.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<OtherControlEnvironment>> CreateManyAsync(IEnumerable<OtherControlEnvironment> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var c in list)
            {
                c.Id = null;
                  if (c.Date == default) // if not provided, set it
                            c.Date = DateTime.UtcNow;
            }

            await _other.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<OtherControlEnvironment> items)
        {
            long modified = 0;

            foreach (var c in items)
            {
                if (string.IsNullOrWhiteSpace(c.Id))
                    continue;

                var result = await _other.ReplaceOneAsync(
                    x => x.Id == c.Id,
                    c
                );

                modified += result.ModifiedCount;
            }

            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;

            var filter = Builders<OtherControlEnvironment>.Filter.In(c => c.Id!, idList);
            var result = await _other.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, OtherControlEnvironment updated)
        {
            var filter = Builders<OtherControlEnvironment>.Filter.Eq(c => c.No, no);

            var update = Builders<OtherControlEnvironment>.Update
                .Set(c => c.Process,                    updated.Process)
                .Set(c => c.ResponsibilityDelegationMatrix, updated.ResponsibilityDelegationMatrix)
                .Set(c => c.SegregationOfDuties,       updated.SegregationOfDuties)
                .Set(c => c.ReportingLines,            updated.ReportingLines)
                .Set(c => c.Mission,                   updated.Mission)
                .Set(c => c.VisionAndValues,           updated.VisionAndValues)
                .Set(c => c.GoalsAndObjectives,        updated.GoalsAndObjectives)
                .Set(c => c.StructuresAndSystems,      updated.StructuresAndSystems)
                .Set(c => c.PoliciesAndProcedures,     updated.PoliciesAndProcedures)
                .Set(c => c.Processes,                 updated.Processes)
                .Set(c => c.IntegrityAndEthicalValues, updated.IntegrityAndEthicalValues)
                .Set(c => c.OversightStructure,        updated.OversightStructure)
                .Set(c => c.Standards,                 updated.Standards)
                .Set(c => c.Methodologies,             updated.Methodologies)
                .Set(c => c.RulesAndRegulations,       updated.RulesAndRegulations);

            var result = await _other.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<OtherControlEnvironment> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<OtherControlEnvironment>.Filter.Eq(c => c.No, item.No);

                var update = Builders<OtherControlEnvironment>.Update
                    .Set(c => c.Process,                    item.Process)
                    .Set(c => c.ResponsibilityDelegationMatrix, item.ResponsibilityDelegationMatrix)
                    .Set(c => c.SegregationOfDuties,       item.SegregationOfDuties)
                    .Set(c => c.ReportingLines,            item.ReportingLines)
                    .Set(c => c.Mission,                   item.Mission)
                    .Set(c => c.VisionAndValues,           item.VisionAndValues)
                    .Set(c => c.GoalsAndObjectives,        item.GoalsAndObjectives)
                    .Set(c => c.StructuresAndSystems,      item.StructuresAndSystems)
                    .Set(c => c.PoliciesAndProcedures,     item.PoliciesAndProcedures)
                    .Set(c => c.Processes,                 item.Processes)
                    .Set(c => c.IntegrityAndEthicalValues, item.IntegrityAndEthicalValues)
                    .Set(c => c.OversightStructure,        item.OversightStructure)
                    .Set(c => c.Standards,                 item.Standards)
                    .Set(c => c.Methodologies,             item.Methodologies)
                    .Set(c => c.RulesAndRegulations,       item.RulesAndRegulations);

                var result = await _other.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
