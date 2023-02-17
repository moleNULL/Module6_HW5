using WebAPI_UnitTests.Data;

namespace WebAPI_UnitTests.Repositories.Interfaces
{
    public interface ICatalogTypeRepository
    {
        Task<int?> AddAsync(string type);
        Task<EntityModifyState> RemoveAsync(int id);
        Task<EntityModifyState> UpdateAsync(int id, string type);
    }
}
