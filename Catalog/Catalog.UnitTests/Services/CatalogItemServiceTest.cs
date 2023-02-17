using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Implementations;
using WebAPI_UnitTests.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly ICatalogItemService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItemEntity _testItem = new CatalogItemEntity()
    {
        Id = 5,
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png"
    };

    public CatalogItemServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.AddAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.AddAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.AddAsync(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task RemoveAsync_Success()
    {
        // arrange
        EntityModifyState expectedResult = EntityModifyState.Deleted;

        _catalogItemRepository.Setup(s => s.RemoveAsync(
            It.IsAny<int>())).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogService.RemoveAsync(_testItem.Id);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task RemoveAsync_Failed()
    {
        // arrange
        int testedId = -4;
        EntityModifyState expectedResult = EntityModifyState.NotFound;

        _catalogItemRepository.Setup(s => s.RemoveAsync(
            It.Is<int>(i => i == testedId))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogService.RemoveAsync(testedId);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        EntityModifyState expectedResult = EntityModifyState.Updated;

        _catalogItemRepository.Setup(s => s.UpdateAsync(
            It.Is<int>(i => i == _testItem.Id),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogService.UpdateAsync(_testItem.Id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int testedId = -4;
        EntityModifyState expectedResult = EntityModifyState.NotFound;

        _catalogItemRepository.Setup(s => s.UpdateAsync(
            It.Is<int>(i => i == testedId),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogService.UpdateAsync(testedId, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        actualResult.Should().Be(expectedResult);
    }
}