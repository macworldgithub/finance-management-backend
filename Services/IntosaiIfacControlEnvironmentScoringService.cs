using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class IntosaiIfacControlEnvironmentScoringService
    {
        private readonly IMongoCollection<IntosaiIfacControlEnvironmentScoring> _collection;

        public IntosaiIfacControlEnvironmentScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<IntosaiIfacControlEnvironmentScoring>("IntosaiIfacControlEnvironmentScoring");
        }

        public async Task<PagedResult<IntosaiIfacControlEnvironmentScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10,
            bool sortByNoAsc = false)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new BsonRegularExpression(search, "i");
                filter = Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Or(
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.Process, regex),

                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityEthicalValues, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommitmentToCompetence, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ManagementPhilosophy, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.OrganizationalStructure, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AssignmentOfAuthority, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.HumanResourcePolicies, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.BoardParticipation, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ManagementControlMethods, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ExternalInfluences, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ManagementCommitmentToIc, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommunicationEthicalValues, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.EmployeeAwareness, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AccountabilityPerformance, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.TransparencyCommitment, regex),

                    // All total scores and ratings
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CompetenceTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.PhilosophyTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.OrgStructureTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AuthorityTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.HrTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.BoardTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ControlMethodsTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ExternalTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommitmentIcTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommEthicalTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AwarenessTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AccountabilityTotalScore, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.TransparencyTotalScore, regex),

                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.IntegrityRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CompetenceRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.PhilosophyRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.OrgStructureRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AuthorityRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.HrRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.BoardRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ControlMethodsRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.ExternalRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommitmentIcRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.CommEthicalRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AwarenessRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.AccountabilityRating, regex),
                    Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Regex(x => x.TransparencyRating, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            IFindFluent<IntosaiIfacControlEnvironmentScoring, IntosaiIfacControlEnvironmentScoring> query = _collection.Find(filter);

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

            return new PagedResult<IntosaiIfacControlEnvironmentScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<IntosaiIfacControlEnvironmentScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IntosaiIfacControlEnvironmentScoring> CreateAsync(IntosaiIfacControlEnvironmentScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, IntosaiIfacControlEnvironmentScoring item)
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

        public async Task<List<IntosaiIfacControlEnvironmentScoring>> CreateManyAsync(IEnumerable<IntosaiIfacControlEnvironmentScoring> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<IntosaiIfacControlEnvironmentScoring> items)
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
            var filter = Builders<IntosaiIfacControlEnvironmentScoring>.Filter.In(x => x.Id!, idList);
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public async Task<bool> UpdateByNoAsync(double no, IntosaiIfacControlEnvironmentScoring updated)
        {
            var filter = Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Eq(x => x.No, no);

            var update = Builders<IntosaiIfacControlEnvironmentScoring>.Update
                .Set(x => x.Process, updated.Process)

                .Set(x => x.IntegrityEthicalValues, updated.IntegrityEthicalValues)
                .Set(x => x.IntegrityDesignScore, updated.IntegrityDesignScore)
                .Set(x => x.IntegrityPerformanceScore, updated.IntegrityPerformanceScore)
                .Set(x => x.IntegritySustainabilityScore, updated.IntegritySustainabilityScore)
                .Set(x => x.IntegrityTotalScore, updated.IntegrityTotalScore)
                .Set(x => x.IntegrityScale, updated.IntegrityScale)
                .Set(x => x.IntegrityRating, updated.IntegrityRating)

                .Set(x => x.CommitmentToCompetence, updated.CommitmentToCompetence)
                .Set(x => x.CompetenceDesignScore, updated.CompetenceDesignScore)
                .Set(x => x.CompetencePerformanceScore, updated.CompetencePerformanceScore)
                .Set(x => x.CompetenceSustainabilityScore, updated.CompetenceSustainabilityScore)
                .Set(x => x.CompetenceTotalScore, updated.CompetenceTotalScore)
                .Set(x => x.CompetenceScale, updated.CompetenceScale)
                .Set(x => x.CompetenceRating, updated.CompetenceRating)

                .Set(x => x.ManagementPhilosophy, updated.ManagementPhilosophy)
                .Set(x => x.PhilosophyDesignScore, updated.PhilosophyDesignScore)
                .Set(x => x.PhilosophyPerformanceScore, updated.PhilosophyPerformanceScore)
                .Set(x => x.PhilosophySustainabilityScore, updated.PhilosophySustainabilityScore)
                .Set(x => x.PhilosophyTotalScore, updated.PhilosophyTotalScore)
                .Set(x => x.PhilosophyScale, updated.PhilosophyScale)
                .Set(x => x.PhilosophyRating, updated.PhilosophyRating)

                .Set(x => x.OrganizationalStructure, updated.OrganizationalStructure)
                .Set(x => x.OrgStructureDesignScore, updated.OrgStructureDesignScore)
                .Set(x => x.OrgStructurePerformanceScore, updated.OrgStructurePerformanceScore)
                .Set(x => x.OrgStructureSustainabilityScore, updated.OrgStructureSustainabilityScore)
                .Set(x => x.OrgStructureTotalScore, updated.OrgStructureTotalScore)
                .Set(x => x.OrgStructureScale, updated.OrgStructureScale)
                .Set(x => x.OrgStructureRating, updated.OrgStructureRating)

                .Set(x => x.AssignmentOfAuthority, updated.AssignmentOfAuthority)
                .Set(x => x.AuthorityDesignScore, updated.AuthorityDesignScore)
                .Set(x => x.AuthorityPerformanceScore, updated.AuthorityPerformanceScore)
                .Set(x => x.AuthoritySustainabilityScore, updated.AuthoritySustainabilityScore)
                .Set(x => x.AuthorityTotalScore, updated.AuthorityTotalScore)
                .Set(x => x.AuthorityScale, updated.AuthorityScale)
                .Set(x => x.AuthorityRating, updated.AuthorityRating)

                .Set(x => x.HumanResourcePolicies, updated.HumanResourcePolicies)
                .Set(x => x.HrDesignScore, updated.HrDesignScore)
                .Set(x => x.HrPerformanceScore, updated.HrPerformanceScore)
                .Set(x => x.HrSustainabilityScore, updated.HrSustainabilityScore)
                .Set(x => x.HrTotalScore, updated.HrTotalScore)
                .Set(x => x.HrScale, updated.HrScale)
                .Set(x => x.HrRating, updated.HrRating)

                .Set(x => x.BoardParticipation, updated.BoardParticipation)
                .Set(x => x.BoardDesignScore, updated.BoardDesignScore)
                .Set(x => x.BoardPerformanceScore, updated.BoardPerformanceScore)
                .Set(x => x.BoardSustainabilityScore, updated.BoardSustainabilityScore)
                .Set(x => x.BoardTotalScore, updated.BoardTotalScore)
                .Set(x => x.BoardScale, updated.BoardScale)
                .Set(x => x.BoardRating, updated.BoardRating)

                .Set(x => x.ManagementControlMethods, updated.ManagementControlMethods)
                .Set(x => x.ControlMethodsDesignScore, updated.ControlMethodsDesignScore)
                .Set(x => x.ControlMethodsPerformanceScore, updated.ControlMethodsPerformanceScore)
                .Set(x => x.ControlMethodsSustainabilityScore, updated.ControlMethodsSustainabilityScore)
                .Set(x => x.ControlMethodsTotalScore, updated.ControlMethodsTotalScore)
                .Set(x => x.ControlMethodsScale, updated.ControlMethodsScale)
                .Set(x => x.ControlMethodsRating, updated.ControlMethodsRating)

                .Set(x => x.ExternalInfluences, updated.ExternalInfluences)
                .Set(x => x.ExternalDesignScore, updated.ExternalDesignScore)
                .Set(x => x.ExternalPerformanceScore, updated.ExternalPerformanceScore)
                .Set(x => x.ExternalSustainabilityScore, updated.ExternalSustainabilityScore)
                .Set(x => x.ExternalTotalScore, updated.ExternalTotalScore)
                .Set(x => x.ExternalScale, updated.ExternalScale)
                .Set(x => x.ExternalRating, updated.ExternalRating)

                .Set(x => x.ManagementCommitmentToIc, updated.ManagementCommitmentToIc)
                .Set(x => x.CommitmentIcDesignScore, updated.CommitmentIcDesignScore)
                .Set(x => x.CommitmentIcPerformanceScore, updated.CommitmentIcPerformanceScore)
                .Set(x => x.CommitmentIcSustainabilityScore, updated.CommitmentIcSustainabilityScore)
                .Set(x => x.CommitmentIcTotalScore, updated.CommitmentIcTotalScore)
                .Set(x => x.CommitmentIcScale, updated.CommitmentIcScale)
                .Set(x => x.CommitmentIcRating, updated.CommitmentIcRating)

                .Set(x => x.CommunicationEthicalValues, updated.CommunicationEthicalValues)
                .Set(x => x.CommEthicalDesignScore, updated.CommEthicalDesignScore)
                .Set(x => x.CommEthicalPerformanceScore, updated.CommEthicalPerformanceScore)
                .Set(x => x.CommEthicalSustainabilityScore, updated.CommEthicalSustainabilityScore)
                .Set(x => x.CommEthicalTotalScore, updated.CommEthicalTotalScore)
                .Set(x => x.CommEthicalScale, updated.CommEthicalScale)
                .Set(x => x.CommEthicalRating, updated.CommEthicalRating)

                .Set(x => x.EmployeeAwareness, updated.EmployeeAwareness)
                .Set(x => x.AwarenessDesignScore, updated.AwarenessDesignScore)
                .Set(x => x.AwarenessPerformanceScore, updated.AwarenessPerformanceScore)
                .Set(x => x.AwarenessSustainabilityScore, updated.AwarenessSustainabilityScore)
                .Set(x => x.AwarenessTotalScore, updated.AwarenessTotalScore)
                .Set(x => x.AwarenessScale, updated.AwarenessScale)
                .Set(x => x.AwarenessRating, updated.AwarenessRating)

                .Set(x => x.AccountabilityPerformance, updated.AccountabilityPerformance)
                .Set(x => x.AccountabilityDesignScore, updated.AccountabilityDesignScore)
                .Set(x => x.AccountabilityPerformanceScore, updated.AccountabilityPerformanceScore)
                .Set(x => x.AccountabilitySustainabilityScore, updated.AccountabilitySustainabilityScore)
                .Set(x => x.AccountabilityTotalScore, updated.AccountabilityTotalScore)
                .Set(x => x.AccountabilityScale, updated.AccountabilityScale)
                .Set(x => x.AccountabilityRating, updated.AccountabilityRating)

                .Set(x => x.TransparencyCommitment, updated.TransparencyCommitment)
                .Set(x => x.TransparencyDesignScore, updated.TransparencyDesignScore)
                .Set(x => x.TransparencyPerformanceScore, updated.TransparencyPerformanceScore)
                .Set(x => x.TransparencySustainabilityScore, updated.TransparencySustainabilityScore)
                .Set(x => x.TransparencyTotalScore, updated.TransparencyTotalScore)
                .Set(x => x.TransparencyScale, updated.TransparencyScale)
                .Set(x => x.TransparencyRating, updated.TransparencyRating);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(IEnumerable<IntosaiIfacControlEnvironmentScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                var filter = Builders<IntosaiIfacControlEnvironmentScoring>.Filter.Eq(x => x.No, item.No);

                var update = Builders<IntosaiIfacControlEnvironmentScoring>.Update
                    .Set(x => x.Process, item.Process)

                    .Set(x => x.IntegrityEthicalValues, item.IntegrityEthicalValues)
                    .Set(x => x.IntegrityDesignScore, item.IntegrityDesignScore)
                    .Set(x => x.IntegrityPerformanceScore, item.IntegrityPerformanceScore)
                    .Set(x => x.IntegritySustainabilityScore, item.IntegritySustainabilityScore)
                    .Set(x => x.IntegrityTotalScore, item.IntegrityTotalScore)
                    .Set(x => x.IntegrityScale, item.IntegrityScale)
                    .Set(x => x.IntegrityRating, item.IntegrityRating)

                    .Set(x => x.CommitmentToCompetence, item.CommitmentToCompetence)
                    .Set(x => x.CompetenceDesignScore, item.CompetenceDesignScore)
                    .Set(x => x.CompetencePerformanceScore, item.CompetencePerformanceScore)
                    .Set(x => x.CompetenceSustainabilityScore, item.CompetenceSustainabilityScore)
                    .Set(x => x.CompetenceTotalScore, item.CompetenceTotalScore)
                    .Set(x => x.CompetenceScale, item.CompetenceScale)
                    .Set(x => x.CompetenceRating, item.CompetenceRating)

                    .Set(x => x.ManagementPhilosophy, item.ManagementPhilosophy)
                    .Set(x => x.PhilosophyDesignScore, item.PhilosophyDesignScore)
                    .Set(x => x.PhilosophyPerformanceScore, item.PhilosophyPerformanceScore)
                    .Set(x => x.PhilosophySustainabilityScore, item.PhilosophySustainabilityScore)
                    .Set(x => x.PhilosophyTotalScore, item.PhilosophyTotalScore)
                    .Set(x => x.PhilosophyScale, item.PhilosophyScale)
                    .Set(x => x.PhilosophyRating, item.PhilosophyRating)

                    .Set(x => x.OrganizationalStructure, item.OrganizationalStructure)
                    .Set(x => x.OrgStructureDesignScore, item.OrgStructureDesignScore)
                    .Set(x => x.OrgStructurePerformanceScore, item.OrgStructurePerformanceScore)
                    .Set(x => x.OrgStructureSustainabilityScore, item.OrgStructureSustainabilityScore)
                    .Set(x => x.OrgStructureTotalScore, item.OrgStructureTotalScore)
                    .Set(x => x.OrgStructureScale, item.OrgStructureScale)
                    .Set(x => x.OrgStructureRating, item.OrgStructureRating)

                    .Set(x => x.AssignmentOfAuthority, item.AssignmentOfAuthority)
                    .Set(x => x.AuthorityDesignScore, item.AuthorityDesignScore)
                    .Set(x => x.AuthorityPerformanceScore, item.AuthorityPerformanceScore)
                    .Set(x => x.AuthoritySustainabilityScore, item.AuthoritySustainabilityScore)
                    .Set(x => x.AuthorityTotalScore, item.AuthorityTotalScore)
                    .Set(x => x.AuthorityScale, item.AuthorityScale)
                    .Set(x => x.AuthorityRating, item.AuthorityRating)

                    .Set(x => x.HumanResourcePolicies, item.HumanResourcePolicies)
                    .Set(x => x.HrDesignScore, item.HrDesignScore)
                    .Set(x => x.HrPerformanceScore, item.HrPerformanceScore)
                    .Set(x => x.HrSustainabilityScore, item.HrSustainabilityScore)
                    .Set(x => x.HrTotalScore, item.HrTotalScore)
                    .Set(x => x.HrScale, item.HrScale)
                    .Set(x => x.HrRating, item.HrRating)

                    .Set(x => x.BoardParticipation, item.BoardParticipation)
                    .Set(x => x.BoardDesignScore, item.BoardDesignScore)
                    .Set(x => x.BoardPerformanceScore, item.BoardPerformanceScore)
                    .Set(x => x.BoardSustainabilityScore, item.BoardSustainabilityScore)
                    .Set(x => x.BoardTotalScore, item.BoardTotalScore)
                    .Set(x => x.BoardScale, item.BoardScale)
                    .Set(x => x.BoardRating, item.BoardRating)

                    .Set(x => x.ManagementControlMethods, item.ManagementControlMethods)
                    .Set(x => x.ControlMethodsDesignScore, item.ControlMethodsDesignScore)
                    .Set(x => x.ControlMethodsPerformanceScore, item.ControlMethodsPerformanceScore)
                    .Set(x => x.ControlMethodsSustainabilityScore, item.ControlMethodsSustainabilityScore)
                    .Set(x => x.ControlMethodsTotalScore, item.ControlMethodsTotalScore)
                    .Set(x => x.ControlMethodsScale, item.ControlMethodsScale)
                    .Set(x => x.ControlMethodsRating, item.ControlMethodsRating)

                    .Set(x => x.ExternalInfluences, item.ExternalInfluences)
                    .Set(x => x.ExternalDesignScore, item.ExternalDesignScore)
                    .Set(x => x.ExternalPerformanceScore, item.ExternalPerformanceScore)
                    .Set(x => x.ExternalSustainabilityScore, item.ExternalSustainabilityScore)
                    .Set(x => x.ExternalTotalScore, item.ExternalTotalScore)
                    .Set(x => x.ExternalScale, item.ExternalScale)
                    .Set(x => x.ExternalRating, item.ExternalRating)

                    .Set(x => x.ManagementCommitmentToIc, item.ManagementCommitmentToIc)
                    .Set(x => x.CommitmentIcDesignScore, item.CommitmentIcDesignScore)
                    .Set(x => x.CommitmentIcPerformanceScore, item.CommitmentIcPerformanceScore)
                    .Set(x => x.CommitmentIcSustainabilityScore, item.CommitmentIcSustainabilityScore)
                    .Set(x => x.CommitmentIcTotalScore, item.CommitmentIcTotalScore)
                    .Set(x => x.CommitmentIcScale, item.CommitmentIcScale)
                    .Set(x => x.CommitmentIcRating, item.CommitmentIcRating)

                    .Set(x => x.CommunicationEthicalValues, item.CommunicationEthicalValues)
                    .Set(x => x.CommEthicalDesignScore, item.CommEthicalDesignScore)
                    .Set(x => x.CommEthicalPerformanceScore, item.CommEthicalPerformanceScore)
                    .Set(x => x.CommEthicalSustainabilityScore, item.CommEthicalSustainabilityScore)
                    .Set(x => x.CommEthicalTotalScore, item.CommEthicalTotalScore)
                    .Set(x => x.CommEthicalScale, item.CommEthicalScale)
                    .Set(x => x.CommEthicalRating, item.CommEthicalRating)

                    .Set(x => x.EmployeeAwareness, item.EmployeeAwareness)
                    .Set(x => x.AwarenessDesignScore, item.AwarenessDesignScore)
                    .Set(x => x.AwarenessPerformanceScore, item.AwarenessPerformanceScore)
                    .Set(x => x.AwarenessSustainabilityScore, item.AwarenessSustainabilityScore)
                    .Set(x => x.AwarenessTotalScore, item.AwarenessTotalScore)
                    .Set(x => x.AwarenessScale, item.AwarenessScale)
                    .Set(x => x.AwarenessRating, item.AwarenessRating)

                    .Set(x => x.AccountabilityPerformance, item.AccountabilityPerformance)
                    .Set(x => x.AccountabilityDesignScore, item.AccountabilityDesignScore)
                    .Set(x => x.AccountabilityPerformanceScore, item.AccountabilityPerformanceScore)
                    .Set(x => x.AccountabilitySustainabilityScore, item.AccountabilitySustainabilityScore)
                    .Set(x => x.AccountabilityTotalScore, item.AccountabilityTotalScore)
                    .Set(x => x.AccountabilityScale, item.AccountabilityScale)
                    .Set(x => x.AccountabilityRating, item.AccountabilityRating)

                    .Set(x => x.TransparencyCommitment, item.TransparencyCommitment)
                    .Set(x => x.TransparencyDesignScore, item.TransparencyDesignScore)
                    .Set(x => x.TransparencyPerformanceScore, item.TransparencyPerformanceScore)
                    .Set(x => x.TransparencySustainabilityScore, item.TransparencySustainabilityScore)
                    .Set(x => x.TransparencyTotalScore, item.TransparencyTotalScore)
                    .Set(x => x.TransparencyScale, item.TransparencyScale)
                    .Set(x => x.TransparencyRating, item.TransparencyRating);

                var result = await _collection.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }
            return modified;
        }
    }
}