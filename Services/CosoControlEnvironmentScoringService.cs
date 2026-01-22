using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class CosoControlEnvironmentScoringService
    {
        private readonly IMongoCollection<CosoControlEnvironmentScoring> _collection;

        public CosoControlEnvironmentScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<CosoControlEnvironmentScoring>("CosoControlEnvironmentScoring");
        }

        public async Task<PagedResult<CosoControlEnvironmentScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10,
            bool sortByNoAsc = false)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<CosoControlEnvironmentScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new BsonRegularExpression(search, "i");
                filter = Builders<CosoControlEnvironmentScoring>.Filter.Or(
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.Process, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityEthicalValues, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.BoardOversight, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.OrganizationalStructure, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.CommitmentToCompetence, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.ManagementPhilosophy, regex),

                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityTotalScore, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.BoardTotalScore, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.OrgStructureTotalScore, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.CompetenceTotalScore, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.PhilosophyTotalScore, regex),

                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityRating, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.BoardRating, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.OrgStructureRating, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.CompetenceRating, regex),
                    Builders<CosoControlEnvironmentScoring>.Filter.Regex(x => x.PhilosophyRating, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            IFindFluent<CosoControlEnvironmentScoring, CosoControlEnvironmentScoring> query = _collection.Find(filter);

            if (sortByNoAsc)
            {
                query = query.SortBy(x => x.No);
            }
            else
            {
                query = query
                    .SortByDescending(x => x.Date)
                    .ThenByDescending(x => x.No);
            }

            var items = await query
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new PagedResult<CosoControlEnvironmentScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<CosoControlEnvironmentScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<CosoControlEnvironmentScoring> CreateAsync(CosoControlEnvironmentScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, CosoControlEnvironmentScoring item)
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

        public async Task<List<CosoControlEnvironmentScoring>> CreateManyAsync(IEnumerable<CosoControlEnvironmentScoring> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<CosoControlEnvironmentScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Id)) continue;
                var result = await _collection.ReplaceOneAsync(x => x.Id == item.Id, item);
                modified += result.ModifiedCount;
            }
            return modified;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> ids)
        {
            var idList = ids.Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            if (idList.Count == 0) return 0;
            var filter = Builders<CosoControlEnvironmentScoring>.Filter.In(x => x.Id!, idList);
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public async Task<bool> UpdateByNoAsync(double no, CosoControlEnvironmentScoring updated)
        {
            var filter = Builders<CosoControlEnvironmentScoring>.Filter.Eq(x => x.No, no);

            var update = Builders<CosoControlEnvironmentScoring>.Update
                .Set(x => x.Process,                          updated.Process)

                .Set(x => x.IntegrityEthicalValues,           updated.IntegrityEthicalValues)
                .Set(x => x.IntegrityDesignScore,             updated.IntegrityDesignScore)
                .Set(x => x.IntegrityPerformanceScore,        updated.IntegrityPerformanceScore)
                .Set(x => x.IntegritySustainabilityScore,     updated.IntegritySustainabilityScore)
                .Set(x => x.IntegrityTotalScore,              updated.IntegrityTotalScore)
                .Set(x => x.IntegrityScale,                   updated.IntegrityScale)
                .Set(x => x.IntegrityRating,                  updated.IntegrityRating)

                .Set(x => x.BoardOversight,                   updated.BoardOversight)
                .Set(x => x.BoardDesignScore,                 updated.BoardDesignScore)
                .Set(x => x.BoardPerformanceScore,            updated.BoardPerformanceScore)
                .Set(x => x.BoardSustainabilityScore,         updated.BoardSustainabilityScore)
                .Set(x => x.BoardTotalScore,                  updated.BoardTotalScore)
                .Set(x => x.BoardScale,                       updated.BoardScale)
                .Set(x => x.BoardRating,                      updated.BoardRating)

                .Set(x => x.OrganizationalStructure,          updated.OrganizationalStructure)
                .Set(x => x.OrgStructureDesignScore,          updated.OrgStructureDesignScore)
                .Set(x => x.OrgStructurePerformanceScore,     updated.OrgStructurePerformanceScore)
                .Set(x => x.OrgStructureSustainabilityScore,  updated.OrgStructureSustainabilityScore)
                .Set(x => x.OrgStructureTotalScore,           updated.OrgStructureTotalScore)
                .Set(x => x.OrgStructureScale,                updated.OrgStructureScale)
                .Set(x => x.OrgStructureRating,               updated.OrgStructureRating)

                .Set(x => x.CommitmentToCompetence,           updated.CommitmentToCompetence)
                .Set(x => x.CompetenceDesignScore,            updated.CompetenceDesignScore)
                .Set(x => x.CompetencePerformanceScore,       updated.CompetencePerformanceScore)
                .Set(x => x.CompetenceSustainabilityScore,    updated.CompetenceSustainabilityScore)
                .Set(x => x.CompetenceTotalScore,             updated.CompetenceTotalScore)
                .Set(x => x.CompetenceScale,                  updated.CompetenceScale)
                .Set(x => x.CompetenceRating,                 updated.CompetenceRating)

                .Set(x => x.ManagementPhilosophy,             updated.ManagementPhilosophy)
                .Set(x => x.PhilosophyDesignScore,            updated.PhilosophyDesignScore)
                .Set(x => x.PhilosophyPerformanceScore,       updated.PhilosophyPerformanceScore)
                .Set(x => x.PhilosophySustainabilityScore,    updated.PhilosophySustainabilityScore)
                .Set(x => x.PhilosophyTotalScore,             updated.PhilosophyTotalScore)
                .Set(x => x.PhilosophyScale,                  updated.PhilosophyScale)
                .Set(x => x.PhilosophyRating,                 updated.PhilosophyRating);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(IEnumerable<CosoControlEnvironmentScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                var filter = Builders<CosoControlEnvironmentScoring>.Filter.Eq(x => x.No, item.No);

                var update = Builders<CosoControlEnvironmentScoring>.Update
                    .Set(x => x.Process,                          item.Process)

                    .Set(x => x.IntegrityEthicalValues,           item.IntegrityEthicalValues)
                    .Set(x => x.IntegrityDesignScore,             item.IntegrityDesignScore)
                    .Set(x => x.IntegrityPerformanceScore,        item.IntegrityPerformanceScore)
                    .Set(x => x.IntegritySustainabilityScore,     item.IntegritySustainabilityScore)
                    .Set(x => x.IntegrityTotalScore,              item.IntegrityTotalScore)
                    .Set(x => x.IntegrityScale,                   item.IntegrityScale)
                    .Set(x => x.IntegrityRating,                  item.IntegrityRating)

                    .Set(x => x.BoardOversight,                   item.BoardOversight)
                    .Set(x => x.BoardDesignScore,                 item.BoardDesignScore)
                    .Set(x => x.BoardPerformanceScore,            item.BoardPerformanceScore)
                    .Set(x => x.BoardSustainabilityScore,         item.BoardSustainabilityScore)
                    .Set(x => x.BoardTotalScore,                  item.BoardTotalScore)
                    .Set(x => x.BoardScale,                       item.BoardScale)
                    .Set(x => x.BoardRating,                      item.BoardRating)

                    .Set(x => x.OrganizationalStructure,          item.OrganizationalStructure)
                    .Set(x => x.OrgStructureDesignScore,          item.OrgStructureDesignScore)
                    .Set(x => x.OrgStructurePerformanceScore,     item.OrgStructurePerformanceScore)
                    .Set(x => x.OrgStructureSustainabilityScore,  item.OrgStructureSustainabilityScore)
                    .Set(x => x.OrgStructureTotalScore,           item.OrgStructureTotalScore)
                    .Set(x => x.OrgStructureScale,                item.OrgStructureScale)
                    .Set(x => x.OrgStructureRating,               item.OrgStructureRating)

                    .Set(x => x.CommitmentToCompetence,           item.CommitmentToCompetence)
                    .Set(x => x.CompetenceDesignScore,            item.CompetenceDesignScore)
                    .Set(x => x.CompetencePerformanceScore,       item.CompetencePerformanceScore)
                    .Set(x => x.CompetenceSustainabilityScore,    item.CompetenceSustainabilityScore)
                    .Set(x => x.CompetenceTotalScore,             item.CompetenceTotalScore)
                    .Set(x => x.CompetenceScale,                  item.CompetenceScale)
                    .Set(x => x.CompetenceRating,                 item.CompetenceRating)

                    .Set(x => x.ManagementPhilosophy,             item.ManagementPhilosophy)
                    .Set(x => x.PhilosophyDesignScore,            item.PhilosophyDesignScore)
                    .Set(x => x.PhilosophyPerformanceScore,       item.PhilosophyPerformanceScore)
                    .Set(x => x.PhilosophySustainabilityScore,    item.PhilosophySustainabilityScore)
                    .Set(x => x.PhilosophyTotalScore,             item.PhilosophyTotalScore)
                    .Set(x => x.PhilosophyScale,                  item.PhilosophyScale)
                    .Set(x => x.PhilosophyRating,                 item.PhilosophyRating);

                var result = await _collection.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }
            return modified;
        }
    }
}