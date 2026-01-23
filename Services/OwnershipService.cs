using MongoDB.Driver;
using finance_management_backend.Models;
using MongoDB.Bson;

namespace finance_management_backend.Services
{
    public class OwnershipScoringService
    {
        private readonly IMongoCollection<OwnershipScoring> _collection;

        public OwnershipScoringService(IMongoDatabase database)
        {
            _collection = database.GetCollection<OwnershipScoring>("OwnershipScoring");
        }

        public async Task<PagedResult<OwnershipScoring>> GetAllAsync(
            int page = 1,
            string? search = null,
            int pageSize = 10,
            bool sortByNoAsc = false)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var filter = Builders<OwnershipScoring>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var regex = new BsonRegularExpression(search, "i");
                filter = Builders<OwnershipScoring>.Filter.Or(
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Activity, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Process, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.ProcessStage, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.ActivationProcess, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Function, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.ClientSegmentOrFunctionalSegment, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.OperationalUnit, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Division, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Entity, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.UnitOrDepartment, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.ProductClass, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.ProductName, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.TotalScore, regex),
                    Builders<OwnershipScoring>.Filter.Regex(x => x.Rating, regex)
                );
            }

            var totalItems = await _collection.CountDocumentsAsync(filter);

            IFindFluent<OwnershipScoring, OwnershipScoring> query = _collection.Find(filter);

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

            return new PagedResult<OwnershipScoring>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        public async Task<OwnershipScoring?> GetByIdAsync(string id)
            => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<OwnershipScoring> CreateAsync(OwnershipScoring item)
        {
            item.Id = null;
            item.Date = DateTime.UtcNow;
            await _collection.InsertOneAsync(item);
            return item;
        }

        public async Task<bool> UpdateAsync(string id, OwnershipScoring item)
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

        public async Task<List<OwnershipScoring>> CreateManyAsync(IEnumerable<OwnershipScoring> items)
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

        public async Task<long> UpdateManyAsync(IEnumerable<OwnershipScoring> items)
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
            var filter = Builders<OwnershipScoring>.Filter.In(x => x.Id!, idList);
            var result = await _collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        // ─── Update using No (business key) ─────────────────────────────────────
        public async Task<bool> UpdateByNoAsync(double no, OwnershipScoring updated)
        {
            var filter = Builders<OwnershipScoring>.Filter.Eq(x => x.No, no);

            var update = Builders<OwnershipScoring>.Update
                .Set(x => x.Activity, updated.Activity)
                .Set(x => x.ActivityScore, updated.ActivityScore)
                .Set(x => x.Process, updated.Process)
                .Set(x => x.ProcessScore, updated.ProcessScore)
                .Set(x => x.ProcessStage, updated.ProcessStage)
                .Set(x => x.ProcessStageScore, updated.ProcessStageScore)
                .Set(x => x.ActivationProcess, updated.ActivationProcess)
                .Set(x => x.ActivationProcessScore, updated.ActivationProcessScore)
                .Set(x => x.Function, updated.Function)
                .Set(x => x.FunctionScore, updated.FunctionScore)
                .Set(x => x.ClientSegmentOrFunctionalSegment, updated.ClientSegmentOrFunctionalSegment)
                .Set(x => x.ClientSegmentScore, updated.ClientSegmentScore)
                .Set(x => x.OperationalUnit, updated.OperationalUnit)
                .Set(x => x.OperationalUnitScore, updated.OperationalUnitScore)
                .Set(x => x.Division, updated.Division)
                .Set(x => x.DivisionScore, updated.DivisionScore)
                .Set(x => x.Entity, updated.Entity)
                .Set(x => x.EntityScore, updated.EntityScore)
                .Set(x => x.UnitOrDepartment, updated.UnitOrDepartment)
                .Set(x => x.UnitOrDepartmentScore, updated.UnitOrDepartmentScore)
                .Set(x => x.ProductClass, updated.ProductClass)
                .Set(x => x.ProductClassScore, updated.ProductClassScore)
                .Set(x => x.ProductName, updated.ProductName)
                .Set(x => x.ProductNameScore, updated.ProductNameScore)
                .Set(x => x.TotalScore, updated.TotalScore)
                .Set(x => x.Scale, updated.Scale)
                .Set(x => x.Rating, updated.Rating);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<long> BulkUpdateByNoAsync(IEnumerable<OwnershipScoring> items)
        {
            long modified = 0;
            foreach (var item in items)
            {
                var filter = Builders<OwnershipScoring>.Filter.Eq(x => x.No, item.No);

                var update = Builders<OwnershipScoring>.Update
                    .Set(x => x.Activity, item.Activity)
                    .Set(x => x.ActivityScore, item.ActivityScore)
                    .Set(x => x.Process, item.Process)
                    .Set(x => x.ProcessScore, item.ProcessScore)
                    .Set(x => x.ProcessStage, item.ProcessStage)
                    .Set(x => x.ProcessStageScore, item.ProcessStageScore)
                    .Set(x => x.ActivationProcess, item.ActivationProcess)
                    .Set(x => x.ActivationProcessScore, item.ActivationProcessScore)
                    .Set(x => x.Function, item.Function)
                    .Set(x => x.FunctionScore, item.FunctionScore)
                    .Set(x => x.ClientSegmentOrFunctionalSegment, item.ClientSegmentOrFunctionalSegment)
                    .Set(x => x.ClientSegmentScore, item.ClientSegmentScore)
                    .Set(x => x.OperationalUnit, item.OperationalUnit)
                    .Set(x => x.OperationalUnitScore, item.OperationalUnitScore)
                    .Set(x => x.Division, item.Division)
                    .Set(x => x.DivisionScore, item.DivisionScore)
                    .Set(x => x.Entity, item.Entity)
                    .Set(x => x.EntityScore, item.EntityScore)
                    .Set(x => x.UnitOrDepartment, item.UnitOrDepartment)
                    .Set(x => x.UnitOrDepartmentScore, item.UnitOrDepartmentScore)
                    .Set(x => x.ProductClass, item.ProductClass)
                    .Set(x => x.ProductClassScore, item.ProductClassScore)
                    .Set(x => x.ProductName, item.ProductName)
                    .Set(x => x.ProductNameScore, item.ProductNameScore)
                    .Set(x => x.TotalScore, item.TotalScore)
                    .Set(x => x.Scale, item.Scale)
                    .Set(x => x.Rating, item.Rating);

                var result = await _collection.UpdateOneAsync(filter, update);
                modified += result.ModifiedCount;
            }
            return modified;
        }
    }
}