using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Implementations;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly CatalogTypeService _catalogTypeService;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;
        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None))
                .ReturnsAsync(dbContextTransaction.Object);

            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            // arrange
            string testedType = "kimono";
            int? expectedResult = 1;

            _catalogTypeRepository.Setup(s => s.AddAsync(
                It.Is<string>(i => i == testedType)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.AddAsync(testedType);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            string testedType = null!;
            int? expectedResult = null;

            _catalogTypeRepository.Setup(s =>
                s.AddAsync(It.Is<string>(i => i == testedType)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.AddAsync(testedType!);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task RemoveAsync_Success()
        {
            // arrange
            int testedId = 5;
            EntityModifyState expectedResult = EntityModifyState.Deleted;

            _catalogTypeRepository.Setup(s =>
                s.RemoveAsync(It.Is<int>(i => i == testedId)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.RemoveAsync(testedId);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task RemoveAsync_Failed()
        {
            // arrange
            int testedId = -4;
            EntityModifyState expectedResult = EntityModifyState.NotFound;

            _catalogTypeRepository.Setup(s =>
                s.RemoveAsync(It.Is<int>(i => i == testedId)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.RemoveAsync(testedId);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            // arrange
            int testedId = 13;
            string testedType = "cheongsam";
            EntityModifyState expectedResult = EntityModifyState.Updated;

            _catalogTypeRepository.Setup(s =>
                s.UpdateAsync(
                    It.Is<int>(i => i == testedId),
                    It.Is<string>(i => i == testedType)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.UpdateAsync(testedId, testedType);

            // assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task UpdateAsync_Failed()
        {
            // arrange
            int testedId = -4;
            string testedType = "cheongsam";
            EntityModifyState expectedResult = EntityModifyState.NotFound;

            _catalogTypeRepository.Setup(s =>
                s.UpdateAsync(
                    It.Is<int>(i => i == testedId),
                    It.Is<string>(i => i == testedType)))
                .ReturnsAsync(expectedResult);

            // act
            var actualResult = await _catalogTypeService.UpdateAsync(testedId, testedType);

            // assert
            actualResult.Should().Be(expectedResult);
        }
    }
}
