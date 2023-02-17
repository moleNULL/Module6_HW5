using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Data.Entities;

namespace WebAPI_UnitTests.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItemEntity>> GetByPageAsync(int pageIndex, int pageSize);
    Task<CatalogItemEntity?> GetByIdAsync(int id);
    Task<List<CatalogItemEntity>?> GetByBrandAsync(string brand);
    Task<List<CatalogItemEntity>?> GetByTypeAsync(string type);
    Task<List<CatalogBrandEntity>> GetBrandsAsync();
    Task<List<CatalogTypeEntity>> GetTypesAsync();
    Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<EntityModifyState> RemoveAsync(int id);
    Task<EntityModifyState> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}