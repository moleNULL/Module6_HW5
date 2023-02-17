using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Interfaces;
using Infrastructure.Services.Implementations;
using Infrastructure.Services.Interfaces;

namespace WebAPI_UnitTests.Services.Implementations;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public Task<int?> AddAsync(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(()
            => _catalogItemRepository.AddAsync(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }

    public Task<EntityModifyState> RemoveAsync(int id)
    {
        return ExecuteSafeAsync(()
            => _catalogItemRepository.RemoveAsync(id));
    }

    public Task<EntityModifyState> UpdateAsync(int id, string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        return ExecuteSafeAsync(()
            => _catalogItemRepository.UpdateAsync(id, name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName));
    }
}