using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Interfaces;
using Infrastructure.Services.Interfaces;

namespace WebAPI_UnitTests.Repositories.Implementations
{
    public class CatalogTypeRepository : ICatalogTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CatalogTypeRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        {
            _dbContext = dbContextWrapper.DbContext;
        }

        public async Task<int?> AddAsync(string type)
        {
            var result = await _dbContext.CatalogTypes.AddAsync(new CatalogTypeEntity { Type = type });

            await _dbContext.SaveChangesAsync();

            return result.Entity.Id;
        }

        public async Task<EntityModifyState> RemoveAsync(int id)
        {
            if (id < 1)
            {
                return EntityModifyState.NotFound;
            }

            bool exists = await _dbContext.CatalogTypes.AnyAsync(ct => ct.Id == id);

            if (!exists)
            {
                return EntityModifyState.NotFound;
            }

            var result = _dbContext.CatalogTypes.Remove(new CatalogTypeEntity { Id = id });
            await _dbContext.SaveChangesAsync();

            return EntityModifyState.Deleted;
        }

        public async Task<EntityModifyState> UpdateAsync(int id, string type)
        {
            if (id < 1)
            {
                return EntityModifyState.NotFound;
            }

            bool exists = await _dbContext.CatalogTypes.AnyAsync(ct => ct.Id == id);

            if (!exists)
            {
                return EntityModifyState.NotFound;
            }

            var result = _dbContext.CatalogTypes.Update(new CatalogTypeEntity { Id = id, Type = type });

            if (result is null)
            {
                return EntityModifyState.NotUpdated;
            }

            await _dbContext.SaveChangesAsync();

            return EntityModifyState.Updated;
        }
    }
}
