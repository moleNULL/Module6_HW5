using WebAPI_UnitTests.Data;

namespace WebAPI_UnitTests.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    Task<EntityModifyState> RemoveAsync(int id);
    Task<EntityModifyState> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}