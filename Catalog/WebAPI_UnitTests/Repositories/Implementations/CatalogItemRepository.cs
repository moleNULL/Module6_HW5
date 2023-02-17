using Infrastructure.Services.Interfaces;
using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Repositories.Interfaces;

namespace WebAPI_UnitTests.Repositories.Implementations;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogItemRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<PaginatedItems<CatalogItemEntity>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItemEntity>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<CatalogItemEntity?> GetByIdAsync(int id)
    {
        if (id < 1)
        {
            return null;
        }

        CatalogItemEntity? item = null;

        try
        {
            item = await _dbContext.CatalogItems
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .FirstAsync(ci => ci.Id == id);
        }
        catch (InvalidOperationException)
        {
            return null;
        }

        return item;
    }

    public async Task<List<CatalogItemEntity>?> GetByBrandAsync(string brand)
    {
        bool exists = await _dbContext.CatalogBrands.AnyAsync(cb => cb.Brand == brand);

        if (!exists)
        {
            return null;
        }

        var items = await _dbContext.CatalogItems
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .Where(ci => ci.CatalogBrand.Brand == brand)
            .ToListAsync();

        return items;
    }

    public async Task<List<CatalogItemEntity>?> GetByTypeAsync(string type)
    {
        bool exists = await _dbContext.CatalogTypes.AnyAsync(ct => ct.Type == type);

        if (!exists)
        {
            return null;
        }

        var items = await _dbContext.CatalogItems
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .Where(ci => ci.CatalogType.Type == type)
            .ToListAsync();

        return items;
    }

    public async Task<List<CatalogBrandEntity>> GetBrandsAsync()
    {
        var brands = await _dbContext.CatalogBrands.ToListAsync();

        return brands;
    }

    public async Task<List<CatalogTypeEntity>> GetTypesAsync()
    {
        var types = await _dbContext.CatalogTypes.ToListAsync();

        return types;
    }

    public async Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItemEntity
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
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

        bool exists = await _dbContext.CatalogItems.AnyAsync(ci => ci.Id == id);

        if (!exists)
        {
            return EntityModifyState.NotFound;
        }

        var result = _dbContext.Remove(new CatalogItemEntity { Id = id });
        await _dbContext.SaveChangesAsync();

        return EntityModifyState.Deleted;
    }

    public async Task<EntityModifyState> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        if (id < 1)
        {
            return EntityModifyState.NotFound;
        }

        bool exists = await _dbContext.CatalogItems.AnyAsync(ci => ci.Id == id);

        if (!exists)
        {
            return EntityModifyState.NotFound;
        }

        var result = _dbContext.CatalogItems.Update(new CatalogItemEntity
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        if (result is null)
        {
            return EntityModifyState.NotUpdated;
        }

        await _dbContext.SaveChangesAsync();

        return EntityModifyState.Updated;
    }
}