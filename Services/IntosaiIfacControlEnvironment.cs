using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class IntosaiIfacControlEnvironmentService
    {
        private readonly IMongoCollection<IntosaiIfacControlEnvironment> _intosai;

        public IntosaiIfacControlEnvironmentService(IMongoDatabase database)
        {
            // collection name in Mongo: "INTOSAI, IFAC, and Government Audit Standards - Control Environment"
            _intosai = database.GetCollection<IntosaiIfacControlEnvironment>(
                "INTOSAI, IFAC, and Government Audit Standards - Control Environment"
            );
        }

        // ===== Single-item CRUD =====

 public async Task<PagedResult<IntosaiIfacControlEnvironment>> GetAllAsync(
    int page = 1,
    string? search = null,
    int pageSize = 10,
    bool sortByNoAsc = false)
{
    if (page < 1) page = 1;
    if (pageSize <= 0) pageSize = 10;

    // ----- Search filter -----
    var filter = Builders<IntosaiIfacControlEnvironment>.Filter.Empty;

    if (!string.IsNullOrWhiteSpace(search))
    {
        var regex = new BsonRegularExpression(search, "i"); // case-insensitive

        filter = Builders<IntosaiIfacControlEnvironment>.Filter.Or(
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.Process, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.IntegrityAndEthicalValues, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.CommitmentToCompetence, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.ManagementsPhilosophyAndOperatingStyle, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.OrganizationalStructure, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.AssignmentOfAuthorityAndResponsibility, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.HumanResourcePoliciesAndPractices, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.BoardOrAuditCommitteeParticipation, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.ManagementControlMethods, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.ExternalInfluences, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.ManagementsCommitmentToInternalControl, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.CommunicationAndEnforcementOfIntegrityAndEthicalValues, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.EmployeeAwarenessAndUnderstanding, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.AccountabilityAndPerformanceMeasurement, regex),
            Builders<IntosaiIfacControlEnvironment>.Filter.Regex(x => x.CommitmentToTransparencyAndOpenness, regex)
        );
    }

    // ----- Count for pagination -----
    var totalItems = await _intosai.CountDocumentsAsync(filter);

    // ----- Build sort definition -----
    IFindFluent<IntosaiIfacControlEnvironment, IntosaiIfacControlEnvironment> query =
        _intosai.Find(filter);

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

    return new PagedResult<IntosaiIfacControlEnvironment>
    {
        Page = page,
        PageSize = pageSize,
        TotalItems = totalItems,
        TotalPages = totalPages,
        Items = items
    };
}


        public async Task<IntosaiIfacControlEnvironment?> GetByIdAsync(string id)
        {
            return await _intosai.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IntosaiIfacControlEnvironment> CreateAsync(IntosaiIfacControlEnvironment item)
        {
            item.Id = null; // let Mongo generate Id
            item.Date = DateTime.UtcNow; // NEW: set current date/time
            await _intosai.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, IntosaiIfacControlEnvironment updated)
        {
            updated.Id = id;
            var result = await _intosai.ReplaceOneAsync(c => c.Id == id, updated);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _intosai.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        // ===== Bulk / many-at-once operations =====

        public async Task<List<IntosaiIfacControlEnvironment>> CreateManyAsync(IEnumerable<IntosaiIfacControlEnvironment> items)
        {
            var list = items.ToList();
            if (list.Count == 0) return list;

            foreach (var c in list)
            {
                c.Id = null;
                  if (c.Date == default) // if not provided, set it
                            c.Date = DateTime.UtcNow;
            }

            await _intosai.InsertManyAsync(list);
            return list;
        }

        public async Task<long> UpdateManyAsync(IEnumerable<IntosaiIfacControlEnvironment> items)
        {
            long modified = 0;

            foreach (var c in items)
            {
                if (string.IsNullOrWhiteSpace(c.Id))
                    continue;

                var result = await _intosai.ReplaceOneAsync(
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

            var filter = Builders<IntosaiIfacControlEnvironment>.Filter.In(c => c.Id!, idList);
            var result = await _intosai.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ===== Update by No (key) – change any attribute except No =====

        // SINGLE: update all attributes except No, using No as key
        public async Task<bool> UpdateByNoAsync(double no, IntosaiIfacControlEnvironment updated)
        {
            var filter = Builders<IntosaiIfacControlEnvironment>.Filter.Eq(c => c.No, no);

            var update = Builders<IntosaiIfacControlEnvironment>.Update
                .Set(c => c.Process,                                   updated.Process)
                .Set(c => c.IntegrityAndEthicalValues,                updated.IntegrityAndEthicalValues)
                .Set(c => c.CommitmentToCompetence,                   updated.CommitmentToCompetence)
                .Set(c => c.ManagementsPhilosophyAndOperatingStyle,   updated.ManagementsPhilosophyAndOperatingStyle)
                .Set(c => c.OrganizationalStructure,                  updated.OrganizationalStructure)
                .Set(c => c.AssignmentOfAuthorityAndResponsibility,   updated.AssignmentOfAuthorityAndResponsibility)
                .Set(c => c.HumanResourcePoliciesAndPractices,        updated.HumanResourcePoliciesAndPractices)
                .Set(c => c.BoardOrAuditCommitteeParticipation,       updated.BoardOrAuditCommitteeParticipation)
                .Set(c => c.ManagementControlMethods,                 updated.ManagementControlMethods)
                .Set(c => c.ExternalInfluences,                       updated.ExternalInfluences)
                .Set(c => c.ManagementsCommitmentToInternalControl,   updated.ManagementsCommitmentToInternalControl)
                .Set(c => c.CommunicationAndEnforcementOfIntegrityAndEthicalValues,
                                                             updated.CommunicationAndEnforcementOfIntegrityAndEthicalValues)
                .Set(c => c.EmployeeAwarenessAndUnderstanding,        updated.EmployeeAwarenessAndUnderstanding)
                .Set(c => c.AccountabilityAndPerformanceMeasurement,  updated.AccountabilityAndPerformanceMeasurement)
                .Set(c => c.CommitmentToTransparencyAndOpenness,      updated.CommitmentToTransparencyAndOpenness);

            var result = await _intosai.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // BULK: for each item, use No as key and update all other fields
        public async Task<long> BulkUpdateByNoAsync(IEnumerable<IntosaiIfacControlEnvironment> items)
        {
            long modified = 0;

            foreach (var item in items)
            {
                var filter = Builders<IntosaiIfacControlEnvironment>.Filter.Eq(c => c.No, item.No);

                var update = Builders<IntosaiIfacControlEnvironment>.Update
                    .Set(c => c.Process,                                   item.Process)
                    .Set(c => c.IntegrityAndEthicalValues,                item.IntegrityAndEthicalValues)
                    .Set(c => c.CommitmentToCompetence,                   item.CommitmentToCompetence)
                    .Set(c => c.ManagementsPhilosophyAndOperatingStyle,   item.ManagementsPhilosophyAndOperatingStyle)
                    .Set(c => c.OrganizationalStructure,                  item.OrganizationalStructure)
                    .Set(c => c.AssignmentOfAuthorityAndResponsibility,   item.AssignmentOfAuthorityAndResponsibility)
                    .Set(c => c.HumanResourcePoliciesAndPractices,        item.HumanResourcePoliciesAndPractices)
                    .Set(c => c.BoardOrAuditCommitteeParticipation,       item.BoardOrAuditCommitteeParticipation)
                    .Set(c => c.ManagementControlMethods,                 item.ManagementControlMethods)
                    .Set(c => c.ExternalInfluences,                       item.ExternalInfluences)
                    .Set(c => c.ManagementsCommitmentToInternalControl,   item.ManagementsCommitmentToInternalControl)
                    .Set(c => c.CommunicationAndEnforcementOfIntegrityAndEthicalValues,
                                                                 item.CommunicationAndEnforcementOfIntegrityAndEthicalValues)
                    .Set(c => c.EmployeeAwarenessAndUnderstanding,        item.EmployeeAwarenessAndUnderstanding)
                    .Set(c => c.AccountabilityAndPerformanceMeasurement,  item.AccountabilityAndPerformanceMeasurement)
                    .Set(c => c.CommitmentToTransparencyAndOpenness,      item.CommitmentToTransparencyAndOpenness);

                var result = await _intosai.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }

            return modified;
        }
    }
}
