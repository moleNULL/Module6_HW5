using WebAPI_UnitTests.Data;

namespace WebAPI_UnitTests.Repositories.Interfaces
{
    public interface ICatalogBrandRepository
    {
        Task<int?> AddAsync(string brand);
        Task<EntityModifyState> RemoveAsync(int id);
        Task<EntityModifyState> UpdateAsync(int id, string brand);
    }
}
