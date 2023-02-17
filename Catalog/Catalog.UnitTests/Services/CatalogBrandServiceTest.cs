using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Implementations;
using WebAPI_UnitTests.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _catalogBrandService;

    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        string testedBrand = "MAUI";
        int expectedResult = 1;

        _catalogBrandRepository.Setup(s => s.AddAsync(
            It.Is<string>(i => i == testedBrand))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.AddAsync(testedBrand);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        string testedBrand = "Ruby-on-Rails";
        int? expectedResult = null;

        _catalogBrandRepository.Setup(s => s.AddAsync(
            It.Is<string>(i => i == testedBrand))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.AddAsync(testedBrand);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task RemoveAsync_Success()
    {
        // arrange
        int testedId = 4;
        EntityModifyState expectedResult = EntityModifyState.Deleted;

        _catalogBrandRepository.Setup(s => s.RemoveAsync(
            It.Is<int>(i => i == testedId))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.RemoveAsync(testedId);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task RemoveAsync_Failed()
    {
        // arrange
        int testedId = -4;
        EntityModifyState expectedResult = EntityModifyState.NotFound;

        _catalogBrandRepository.Setup(s => s.RemoveAsync(
            It.Is<int>(i => i == testedId))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.RemoveAsync(testedId);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        int testedId = 4;
        string testedBrand = "MAUI";
        EntityModifyState expectedResult = EntityModifyState.Updated;

        _catalogBrandRepository.Setup(s => s.UpdateAsync(
            It.Is<int>(i => i == testedId),
            It.Is<string>(i => i == testedBrand))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.UpdateAsync(testedId, testedBrand);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        int testedId = -4;
        string testedBrand = "MAUI";
        EntityModifyState expectedResult = EntityModifyState.NotFound;

        _catalogBrandRepository.Setup(s => s.UpdateAsync(
            It.Is<int>(i => i == testedId),
            It.Is<string>(i => i == testedBrand))).ReturnsAsync(expectedResult);

        // act
        var actualResult = await _catalogBrandService.UpdateAsync(testedId, testedBrand);

        // assert
        actualResult.Should().Be(expectedResult);
    }
}