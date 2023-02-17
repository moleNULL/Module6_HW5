using WebAPI_UnitTests.Data;

namespace WebAPI_UnitTests.Services.Interfaces
{
    public interface ICatalogTypeService
    {
        Task<int?> AddAsync(string type);
        Task<EntityModifyState> RemoveAsync(int id);
        Task<EntityModifyState> UpdateAsync(int id, string type);
    }
}
