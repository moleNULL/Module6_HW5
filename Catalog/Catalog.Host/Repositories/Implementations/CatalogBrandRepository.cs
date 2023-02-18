using Catalog.Host.Data.Entities;
using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Repositories.Implementations
{
    public class CatalogBrandRepository : ICatalogBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CatalogBrandRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        {
            _dbContext = dbContextWrapper.DbContext;
        }

        public async Task<int?> AddAsync(string brand)
        {
            var item = await _dbContext.CatalogBrands.AddAsync(new CatalogBrandEntity
            {
                Brand = brand
            });

            await _dbContext.SaveChangesAsync();

            return item.Entity.Id;
        }

        public async Task<EntityModifyState> RemoveAsync(int id)
        {
            if (id < 1)
            {
                return EntityModifyState.NotFound;
            }

            bool exists = await _dbContext.CatalogBrands.AnyAsync(cb => cb.Id == id);

            if (!exists)
            {
                return EntityModifyState.NotFound;
            }

            var result = _dbContext.CatalogBrands.Remove(new CatalogBrandEntity { Id = id });
            await _dbContext.SaveChangesAsync();

            return EntityModifyState.Deleted;
        }

        public async Task<EntityModifyState> UpdateAsync(int id, string brand)
        {
            if (id < 1)
            {
                return EntityModifyState.NotFound;
            }

            bool exists = await _dbContext.CatalogBrands.AnyAsync(cb => cb.Id == id);

            if (!exists)
            {
                return EntityModifyState.NotFound;
            }

            var result = _dbContext.CatalogBrands.Update(new CatalogBrandEntity { Id = id, Brand = brand });

            if (result is null)
            {
                return EntityModifyState.NotUpdated;
            }

            await _dbContext.SaveChangesAsync();

            return EntityModifyState.Updated;
        }
    }
}
