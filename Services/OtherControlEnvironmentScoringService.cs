using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class OtherControlEnvironmentScoringService
    {
        private readonly IMongoCollection<OtherControlEnvironmentScoring> _collection;

        public OtherControlEnvironmentScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<OtherControlEnvironmentScoring>("OtherControlEnvironmentScoring");
        }

        public async Task<PagedResult<OtherControlEnvironmentScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10,
            bool sortByNoAsc = false)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<OtherControlEnvironmentScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new BsonRegularExpression(search, "i");
                filter = Builders<OtherControlEnvironmentScoring>.Filter.Or(
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.Process, regex),

                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ResponsibilityDelegationMatrix, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.SegregationOfDuties, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ReportingLines, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.Mission, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.VisionAndValues, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.GoalsAndObjectives, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.StructuresAndSystems, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.PoliciesAndProcedures, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.Processes, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityEthicalValues, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.OversightStructure, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.Standards, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.Methodologies, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.RulesAndRegulations, regex),

                    // Total scores
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.RdmTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.SodTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ReportingLinesTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.MissionTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.VisionValuesTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.GoalsObjectivesTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.StructuresSystemsTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.PoliciesProceduresTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ProcessesTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.OversightTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.StandardsTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.MethodologiesTotalScore, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.RulesRegsTotalScore, regex),

                    // Ratings
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.RdmRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.SodRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ReportingLinesRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.MissionRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.VisionValuesRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.GoalsObjectivesRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.StructuresSystemsRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.PoliciesProceduresRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.ProcessesRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.OversightRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.StandardsRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.MethodologiesRating, regex),
                    Builders<OtherControlEnvironmentScoring>.Filter.Regex(x => x.RulesRegsRating, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            IFindFluent<OtherControlEnvironmentScoring, OtherControlEnvironmentScoring> query = _collection.Find(filter);

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

            return new PagedResult<OtherControlEnvironmentScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<OtherControlEnvironmentScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<OtherControlEnvironmentScoring> CreateAsync(OtherControlEnvironmentScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, OtherControlEnvironmentScoring item)
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

        public async Task<List<OtherControlEnvironmentScoring>> CreateManyAsync(IEnumerable<OtherControlEnvironmentScoring> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<OtherControlEnvironmentScoring> items)
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
            var filter = Builders<OtherControlEnvironmentScoring>.Filter.In(x => x.Id!, idList);
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public async Task<bool> UpdateByNoAsync(double no, OtherControlEnvironmentScoring updated)
        {
            var filter = Builders<OtherControlEnvironmentScoring>.Filter.Eq(x => x.No, no);

            var update = Builders<OtherControlEnvironmentScoring>.Update
                .Set(x => x.Process, updated.Process)

                .Set(x => x.ResponsibilityDelegationMatrix, updated.ResponsibilityDelegationMatrix)
                .Set(x => x.RdmDesignScore, updated.RdmDesignScore)
                .Set(x => x.RdmPerformanceScore, updated.RdmPerformanceScore)
                .Set(x => x.RdmSustainabilityScore, updated.RdmSustainabilityScore)
                .Set(x => x.RdmTotalScore, updated.RdmTotalScore)
                .Set(x => x.RdmScale, updated.RdmScale)
                .Set(x => x.RdmRating, updated.RdmRating)

                .Set(x => x.SegregationOfDuties, updated.SegregationOfDuties)
                .Set(x => x.SodDesignScore, updated.SodDesignScore)
                .Set(x => x.SodPerformanceScore, updated.SodPerformanceScore)
                .Set(x => x.SodSustainabilityScore, updated.SodSustainabilityScore)
                .Set(x => x.SodTotalScore, updated.SodTotalScore)
                .Set(x => x.SodScale, updated.SodScale)
                .Set(x => x.SodRating, updated.SodRating)

                .Set(x => x.ReportingLines, updated.ReportingLines)
                .Set(x => x.ReportingLinesDesignScore, updated.ReportingLinesDesignScore)
                .Set(x => x.ReportingLinesPerformanceScore, updated.ReportingLinesPerformanceScore)
                .Set(x => x.ReportingLinesSustainabilityScore, updated.ReportingLinesSustainabilityScore)
                .Set(x => x.ReportingLinesTotalScore, updated.ReportingLinesTotalScore)
                .Set(x => x.ReportingLinesScale, updated.ReportingLinesScale)
                .Set(x => x.ReportingLinesRating, updated.ReportingLinesRating)

                .Set(x => x.Mission, updated.Mission)
                .Set(x => x.MissionDesignScore, updated.MissionDesignScore)
                .Set(x => x.MissionPerformanceScore, updated.MissionPerformanceScore)
                .Set(x => x.MissionSustainabilityScore, updated.MissionSustainabilityScore)
                .Set(x => x.MissionTotalScore, updated.MissionTotalScore)
                .Set(x => x.MissionScale, updated.MissionScale)
                .Set(x => x.MissionRating, updated.MissionRating)

                .Set(x => x.VisionAndValues, updated.VisionAndValues)
                .Set(x => x.VisionValuesDesignScore, updated.VisionValuesDesignScore)
                .Set(x => x.VisionValuesPerformanceScore, updated.VisionValuesPerformanceScore)
                .Set(x => x.VisionValuesSustainabilityScore, updated.VisionValuesSustainabilityScore)
                .Set(x => x.VisionValuesTotalScore, updated.VisionValuesTotalScore)
                .Set(x => x.VisionValuesScale, updated.VisionValuesScale)
                .Set(x => x.VisionValuesRating, updated.VisionValuesRating)

                .Set(x => x.GoalsAndObjectives, updated.GoalsAndObjectives)
                .Set(x => x.GoalsObjectivesDesignScore, updated.GoalsObjectivesDesignScore)
                .Set(x => x.GoalsObjectivesPerformanceScore, updated.GoalsObjectivesPerformanceScore)
                .Set(x => x.GoalsObjectivesSustainabilityScore, updated.GoalsObjectivesSustainabilityScore)
                .Set(x => x.GoalsObjectivesTotalScore, updated.GoalsObjectivesTotalScore)
                .Set(x => x.GoalsObjectivesScale, updated.GoalsObjectivesScale)
                .Set(x => x.GoalsObjectivesRating, updated.GoalsObjectivesRating)

                .Set(x => x.StructuresAndSystems, updated.StructuresAndSystems)
                .Set(x => x.StructuresSystemsDesignScore, updated.StructuresSystemsDesignScore)
                .Set(x => x.StructuresSystemsPerformanceScore, updated.StructuresSystemsPerformanceScore)
                .Set(x => x.StructuresSystemsSustainabilityScore, updated.StructuresSystemsSustainabilityScore)
                .Set(x => x.StructuresSystemsTotalScore, updated.StructuresSystemsTotalScore)
                .Set(x => x.StructuresSystemsScale, updated.StructuresSystemsScale)
                .Set(x => x.StructuresSystemsRating, updated.StructuresSystemsRating)

                .Set(x => x.PoliciesAndProcedures, updated.PoliciesAndProcedures)
                .Set(x => x.PoliciesProceduresDesignScore, updated.PoliciesProceduresDesignScore)
                .Set(x => x.PoliciesProceduresPerformanceScore, updated.PoliciesProceduresPerformanceScore)
                .Set(x => x.PoliciesProceduresSustainabilityScore, updated.PoliciesProceduresSustainabilityScore)
                .Set(x => x.PoliciesProceduresTotalScore, updated.PoliciesProceduresTotalScore)
                .Set(x => x.PoliciesProceduresScale, updated.PoliciesProceduresScale)
                .Set(x => x.PoliciesProceduresRating, updated.PoliciesProceduresRating)

                .Set(x => x.Processes, updated.Processes)
                .Set(x => x.ProcessesDesignScore, updated.ProcessesDesignScore)
                .Set(x => x.ProcessesPerformanceScore, updated.ProcessesPerformanceScore)
                .Set(x => x.ProcessesSustainabilityScore, updated.ProcessesSustainabilityScore)
                .Set(x => x.ProcessesTotalScore, updated.ProcessesTotalScore)
                .Set(x => x.ProcessesScale, updated.ProcessesScale)
                .Set(x => x.ProcessesRating, updated.ProcessesRating)

                .Set(x => x.IntegrityEthicalValues, updated.IntegrityEthicalValues)
                .Set(x => x.IntegrityDesignScore, updated.IntegrityDesignScore)
                .Set(x => x.IntegrityPerformanceScore, updated.IntegrityPerformanceScore)
                .Set(x => x.IntegritySustainabilityScore, updated.IntegritySustainabilityScore)
                .Set(x => x.IntegrityTotalScore, updated.IntegrityTotalScore)
                .Set(x => x.IntegrityScale, updated.IntegrityScale)
                .Set(x => x.IntegrityRating, updated.IntegrityRating)

                .Set(x => x.OversightStructure, updated.OversightStructure)
                .Set(x => x.OversightDesignScore, updated.OversightDesignScore)
                .Set(x => x.OversightPerformanceScore, updated.OversightPerformanceScore)
                .Set(x => x.OversightSustainabilityScore, updated.OversightSustainabilityScore)
                .Set(x => x.OversightTotalScore, updated.OversightTotalScore)
                .Set(x => x.OversightScale, updated.OversightScale)
                .Set(x => x.OversightRating, updated.OversightRating)

                .Set(x => x.Standards, updated.Standards)
                .Set(x => x.StandardsDesignScore, updated.StandardsDesignScore)
                .Set(x => x.StandardsPerformanceScore, updated.StandardsPerformanceScore)
                .Set(x => x.StandardsSustainabilityScore, updated.StandardsSustainabilityScore)
                .Set(x => x.StandardsTotalScore, updated.StandardsTotalScore)
                .Set(x => x.StandardsScale, updated.StandardsScale)
                .Set(x => x.StandardsRating, updated.StandardsRating)

                .Set(x => x.Methodologies, updated.Methodologies)
                .Set(x => x.MethodologiesDesignScore, updated.MethodologiesDesignScore)
                .Set(x => x.MethodologiesPerformanceScore, updated.MethodologiesPerformanceScore)
                .Set(x => x.MethodologiesSustainabilityScore, updated.MethodologiesSustainabilityScore)
                .Set(x => x.MethodologiesTotalScore, updated.MethodologiesTotalScore)
                .Set(x => x.MethodologiesScale, updated.MethodologiesScale)
                .Set(x => x.MethodologiesRating, updated.MethodologiesRating)

                .Set(x => x.RulesAndRegulations, updated.RulesAndRegulations)
                .Set(x => x.RulesRegsDesignScore, updated.RulesRegsDesignScore)
                .Set(x => x.RulesRegsPerformanceScore, updated.RulesRegsPerformanceScore)
                .Set(x => x.RulesRegsSustainabilityScore, updated.RulesRegsSustainabilityScore)
                .Set(x => x.RulesRegsTotalScore, updated.RulesRegsTotalScore)
                .Set(x => x.RulesRegsScale, updated.RulesRegsScale)
                .Set(x => x.RulesRegsRating, updated.RulesRegsRating);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(IEnumerable<OtherControlEnvironmentScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                var filter = Builders<OtherControlEnvironmentScoring>.Filter.Eq(x => x.No, item.No);

                var update = Builders<OtherControlEnvironmentScoring>.Update
                    .Set(x => x.Process, item.Process)

                    .Set(x => x.ResponsibilityDelegationMatrix, item.ResponsibilityDelegationMatrix)
                    .Set(x => x.RdmDesignScore, item.RdmDesignScore)
                    .Set(x => x.RdmPerformanceScore, item.RdmPerformanceScore)
                    .Set(x => x.RdmSustainabilityScore, item.RdmSustainabilityScore)
                    .Set(x => x.RdmTotalScore, item.RdmTotalScore)
                    .Set(x => x.RdmScale, item.RdmScale)
                    .Set(x => x.RdmRating, item.RdmRating)

                    .Set(x => x.SegregationOfDuties, item.SegregationOfDuties)
                    .Set(x => x.SodDesignScore, item.SodDesignScore)
                    .Set(x => x.SodPerformanceScore, item.SodPerformanceScore)
                    .Set(x => x.SodSustainabilityScore, item.SodSustainabilityScore)
                    .Set(x => x.SodTotalScore, item.SodTotalScore)
                    .Set(x => x.SodScale, item.SodScale)
                    .Set(x => x.SodRating, item.SodRating)

                    .Set(x => x.ReportingLines, item.ReportingLines)
                    .Set(x => x.ReportingLinesDesignScore, item.ReportingLinesDesignScore)
                    .Set(x => x.ReportingLinesPerformanceScore, item.ReportingLinesPerformanceScore)
                    .Set(x => x.ReportingLinesSustainabilityScore, item.ReportingLinesSustainabilityScore)
                    .Set(x => x.ReportingLinesTotalScore, item.ReportingLinesTotalScore)
                    .Set(x => x.ReportingLinesScale, item.ReportingLinesScale)
                    .Set(x => x.ReportingLinesRating, item.ReportingLinesRating)

                    .Set(x => x.Mission, item.Mission)
                    .Set(x => x.MissionDesignScore, item.MissionDesignScore)
                    .Set(x => x.MissionPerformanceScore, item.MissionPerformanceScore)
                    .Set(x => x.MissionSustainabilityScore, item.MissionSustainabilityScore)
                    .Set(x => x.MissionTotalScore, item.MissionTotalScore)
                    .Set(x => x.MissionScale, item.MissionScale)
                    .Set(x => x.MissionRating, item.MissionRating)

                    .Set(x => x.VisionAndValues, item.VisionAndValues)
                    .Set(x => x.VisionValuesDesignScore, item.VisionValuesDesignScore)
                    .Set(x => x.VisionValuesPerformanceScore, item.VisionValuesPerformanceScore)
                    .Set(x => x.VisionValuesSustainabilityScore, item.VisionValuesSustainabilityScore)
                    .Set(x => x.VisionValuesTotalScore, item.VisionValuesTotalScore)
                    .Set(x => x.VisionValuesScale, item.VisionValuesScale)
                    .Set(x => x.VisionValuesRating, item.VisionValuesRating)

                    .Set(x => x.GoalsAndObjectives, item.GoalsAndObjectives)
                    .Set(x => x.GoalsObjectivesDesignScore, item.GoalsObjectivesDesignScore)
                    .Set(x => x.GoalsObjectivesPerformanceScore, item.GoalsObjectivesPerformanceScore)
                    .Set(x => x.GoalsObjectivesSustainabilityScore, item.GoalsObjectivesSustainabilityScore)
                    .Set(x => x.GoalsObjectivesTotalScore, item.GoalsObjectivesTotalScore)
                    .Set(x => x.GoalsObjectivesScale, item.GoalsObjectivesScale)
                    .Set(x => x.GoalsObjectivesRating, item.GoalsObjectivesRating)

                    .Set(x => x.StructuresAndSystems, item.StructuresAndSystems)
                    .Set(x => x.StructuresSystemsDesignScore, item.StructuresSystemsDesignScore)
                    .Set(x => x.StructuresSystemsPerformanceScore, item.StructuresSystemsPerformanceScore)
                    .Set(x => x.StructuresSystemsSustainabilityScore, item.StructuresSystemsSustainabilityScore)
                    .Set(x => x.StructuresSystemsTotalScore, item.StructuresSystemsTotalScore)
                    .Set(x => x.StructuresSystemsScale, item.StructuresSystemsScale)
                    .Set(x => x.StructuresSystemsRating, item.StructuresSystemsRating)

                    .Set(x => x.PoliciesAndProcedures, item.PoliciesAndProcedures)
                    .Set(x => x.PoliciesProceduresDesignScore, item.PoliciesProceduresDesignScore)
                    .Set(x => x.PoliciesProceduresPerformanceScore, item.PoliciesProceduresPerformanceScore)
                    .Set(x => x.PoliciesProceduresSustainabilityScore, item.PoliciesProceduresSustainabilityScore)
                    .Set(x => x.PoliciesProceduresTotalScore, item.PoliciesProceduresTotalScore)
                    .Set(x => x.PoliciesProceduresScale, item.PoliciesProceduresScale)
                    .Set(x => x.PoliciesProceduresRating, item.PoliciesProceduresRating)

                    .Set(x => x.Processes, item.Processes)
                    .Set(x => x.ProcessesDesignScore, item.ProcessesDesignScore)
                    .Set(x => x.ProcessesPerformanceScore, item.ProcessesPerformanceScore)
                    .Set(x => x.ProcessesSustainabilityScore, item.ProcessesSustainabilityScore)
                    .Set(x => x.ProcessesTotalScore, item.ProcessesTotalScore)
                    .Set(x => x.ProcessesScale, item.ProcessesScale)
                    .Set(x => x.ProcessesRating, item.ProcessesRating)

                    .Set(x => x.IntegrityEthicalValues, item.IntegrityEthicalValues)
                    .Set(x => x.IntegrityDesignScore, item.IntegrityDesignScore)
                    .Set(x => x.IntegrityPerformanceScore, item.IntegrityPerformanceScore)
                    .Set(x => x.IntegritySustainabilityScore, item.IntegritySustainabilityScore)
                    .Set(x => x.IntegrityTotalScore, item.IntegrityTotalScore)
                    .Set(x => x.IntegrityScale, item.IntegrityScale)
                    .Set(x => x.IntegrityRating, item.IntegrityRating)

                    .Set(x => x.OversightStructure, item.OversightStructure)
                    .Set(x => x.OversightDesignScore, item.OversightDesignScore)
                    .Set(x => x.OversightPerformanceScore, item.OversightPerformanceScore)
                    .Set(x => x.OversightSustainabilityScore, item.OversightSustainabilityScore)
                    .Set(x => x.OversightTotalScore, item.OversightTotalScore)
                    .Set(x => x.OversightScale, item.OversightScale)
                    .Set(x => x.OversightRating, item.OversightRating)

                    .Set(x => x.Standards, item.Standards)
                    .Set(x => x.StandardsDesignScore, item.StandardsDesignScore)
                    .Set(x => x.StandardsPerformanceScore, item.StandardsPerformanceScore)
                    .Set(x => x.StandardsSustainabilityScore, item.StandardsSustainabilityScore)
                    .Set(x => x.StandardsTotalScore, item.StandardsTotalScore)
                    .Set(x => x.StandardsScale, item.StandardsScale)
                    .Set(x => x.StandardsRating, item.StandardsRating)

                    .Set(x => x.Methodologies, item.Methodologies)
                    .Set(x => x.MethodologiesDesignScore, item.MethodologiesDesignScore)
                    .Set(x => x.MethodologiesPerformanceScore, item.MethodologiesPerformanceScore)
                    .Set(x => x.MethodologiesSustainabilityScore, item.MethodologiesSustainabilityScore)
                    .Set(x => x.MethodologiesTotalScore, item.MethodologiesTotalScore)
                    .Set(x => x.MethodologiesScale, item.MethodologiesScale)
                    .Set(x => x.MethodologiesRating, item.MethodologiesRating)

                    .Set(x => x.RulesAndRegulations, item.RulesAndRegulations)
                    .Set(x => x.RulesRegsDesignScore, item.RulesRegsDesignScore)
                    .Set(x => x.RulesRegsPerformanceScore, item.RulesRegsPerformanceScore)
                    .Set(x => x.RulesRegsSustainabilityScore, item.RulesRegsSustainabilityScore)
                    .Set(x => x.RulesRegsTotalScore, item.RulesRegsTotalScore)
                    .Set(x => x.RulesRegsScale, item.RulesRegsScale)
                    .Set(x => x.RulesRegsRating, item.RulesRegsRating);

                var result = await _collection.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }
            return modified;
        }
    }
}