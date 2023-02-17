using WebAPI_UnitTests.Data;

namespace WebAPI_UnitTests.Services.Interfaces
{
    public interface ICatalogBrandService
    {
        Task<int?> AddAsync(string brand);
        Task<EntityModifyState> RemoveAsync(int id);
        Task<EntityModifyState> UpdateAsync(int id, string brand);
    }
}
