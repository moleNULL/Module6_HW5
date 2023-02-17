using Infrastructure.Services.Implementations;
using Infrastructure.Services.Interfaces;
using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Interfaces;

namespace WebAPI_UnitTests.Services.Implementations
{
    public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
    {
        private readonly ICatalogTypeRepository _catalogTypeRepository;

        public CatalogTypeService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            ICatalogTypeRepository catalogTypeRepository)
            : base(dbContextWrapper, logger)
        {
            _catalogTypeRepository = catalogTypeRepository;
        }

        public Task<int?> AddAsync(string type)
        {
            return ExecuteSafeAsync(()
                => _catalogTypeRepository.AddAsync(type));
        }

        public Task<EntityModifyState> RemoveAsync(int id)
        {
            return ExecuteSafeAsync(()
                => _catalogTypeRepository.RemoveAsync(id));
        }

        public Task<EntityModifyState> UpdateAsync(int id, string type)
        {
            return ExecuteSafeAsync(()
                => _catalogTypeRepository.UpdateAsync(id, type));
        }
    }
}
