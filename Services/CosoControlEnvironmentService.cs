using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class CosoControlEnvironmentService
    {
        private readonly IMongoCollection<CosoControlEnvironment> _coso;

        public CosoControlEnvironmentService(IMongoDatabase database)
        {
            // collection name in Mongo: "COSO-Control Environment"
            _coso = database.GetCollection<CosoControlEnvironment>("COSO-Control Environment");
        }

        // ===== Single-item CRUD =====

public async Task<PagedResult<CosoControlEnvironment>> GetAllAsync(
    int page = 1,
    string? search = null,
    int pageSize = 10,
    bool sortByNoAsc = false)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // ----- Search filter -----
    var filter = Builders<CosoControlEnvironment>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<CosoControlEnvironment>.Filter.Or(
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.Process, regex),
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.IntegrityAndEthicalValues, regex),
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.BoardOversight, regex),
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.OrganizationalStructure, regex),
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.CommitmentToCompetence, regex),
            Builders<CosoControlEnvironment>.Filter.Regex(x => x.ManagementPhilosophy, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _coso.CountDocumentsAsync(filter);

    // ----- Build sort definition -----
    IFindFluent<CosoControlEnvironment, CosoControlEnvironment> query = _coso.Find(filter);

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

    return new PagedResult<CosoControlEnvironment>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<CosoControlEnvironment?> GetByIdAsync(string id)
        {
            return await _coso.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CosoControlEnvironment> CreateAsync(CosoControlEnvironment item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _coso.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, CosoControlEnvironment updated)
        {
            updated.Id = id;
            var result = await _coso.ReplaceOneAsync(c => c.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _coso.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<CosoControlEnvironment>> CreateManyAsync(IEnumerable<CosoControlEnvironment> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var c in list)
            {
                c.Id = null;
                  if (c.Date == default) // if not provided, set it
                            c.Date = DateTime.UtcNow;
            }

            await _coso.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<CosoControlEnvironment> items)
        {
            long modified = 0;

            foreach (var c in items)
            {
                if (string.IsNullOrWhiteSpace(c.Id))
                    continue;

                var result = await _coso.ReplaceOneAsync(
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

            var filter = Builders<CosoControlEnvironment>.Filter.In(c => c.Id!, idList);
            var result = await _coso.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, CosoControlEnvironment updated)
        {
            var filter = Builders<CosoControlEnvironment>.Filter.Eq(c => c.No, no);

            var update = Builders<CosoControlEnvironment>.Update
                .Set(c => c.Process,                 updated.Process)
                .Set(c => c.IntegrityAndEthicalValues, updated.IntegrityAndEthicalValues)
                .Set(c => c.BoardOversight,         updated.BoardOversight)
                .Set(c => c.OrganizationalStructure,updated.OrganizationalStructure)
                .Set(c => c.CommitmentToCompetence, updated.CommitmentToCompetence)
                .Set(c => c.ManagementPhilosophy,   updated.ManagementPhilosophy);

            var result = await _coso.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<CosoControlEnvironment> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<CosoControlEnvironment>.Filter.Eq(c => c.No, item.No);

                var update = Builders<CosoControlEnvironment>.Update
                    .Set(c => c.Process,                   item.Process)
                    .Set(c => c.IntegrityAndEthicalValues, item.IntegrityAndEthicalValues)
                    .Set(c => c.BoardOversight,            item.BoardOversight)
                    .Set(c => c.OrganizationalStructure,   item.OrganizationalStructure)
                    .Set(c => c.CommitmentToCompetence,    item.CommitmentToCompetence)
                    .Set(c => c.ManagementPhilosophy,      item.ManagementPhilosophy);

                var result = await _coso.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
