using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Models.Dtos;
using WebAPI_UnitTests.Models.Response;
using WebAPI_UnitTests.Repositories.Interfaces;
using WebAPI_UnitTests.Services.Implementations;
using WebAPI_UnitTests.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItemEntity>()
        {
            Data = new List<CatalogItemEntity>()
            {
                new CatalogItemEntity()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItemEntity()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItemEntity>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemByIdAsync_Success()
    {
        // arrange
        int testedId = 5;
        var entities = new CatalogItemEntity
        {
            Id = 5,
            Name = "Item"
        };
        var expectedResultDto = new CatalogItemDto
        {
            Id = 5,
            Name = "Item"
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testedId))).ReturnsAsync(entities);

        _mapper.Setup(s => s.Map(
            It.Is<CatalogItemEntity>(i => i.Equals(entities)),
            typeof(CatalogItemEntity),
            typeof(CatalogItemDto))).Returns(expectedResultDto);

        // act
        var actualResult = await _catalogService.GetItemByIdAsync(testedId);

        // arrange
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResultDto);
    }

    [Fact]
    public async Task GetItemByIdAsync_Failed()
    {
        // arrange
        int testedId = -4;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == testedId))).ReturnsAsync((CatalogItemEntity)null!);

        // act
        var actualResult = await _catalogService.GetItemByIdAsync(testedId);

        // arrange
        actualResult.Should().BeNull();
    }

    [Fact]
    public async Task GetItemsByBrandAsync_Success()
    {
        // arrange
        string testedBrand = "SQL Server";
        var entities = new List<CatalogItemEntity>
        {
            new CatalogItemEntity() { Name = "T-Shirt" }
        };
        var expectedItemsDto = new List<CatalogItemDto>
        {
            new CatalogItemDto() { Name = "T-Shirt" }
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i == testedBrand)))
            .ReturnsAsync(entities);

        _mapper.Setup(s => s.Map(
            It.Is<List<CatalogItemEntity>>(i => i.Equals(entities)),
            typeof(List<CatalogItemEntity>),
            typeof(List<CatalogItemDto>))).Returns(expectedItemsDto);

        // act
        var actualResult = await _catalogService.GetItemsByBrandAsync(testedBrand);

        // arrange
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedItemsDto);
    }

    [Fact]
    public async Task GetItemsByBrandAsync_Failed()
    {
        // arrange
        string testedBrand = null!;

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i == testedBrand)))
            .ReturnsAsync((List<CatalogItemEntity>)null!);

        // act
        var actualResult = await _catalogService.GetItemsByBrandAsync(testedBrand);

        // assert
        actualResult.Should().BeNull();
    }

    [Fact]
    public async Task GetItemsByTypeAsync_Success()
    {
        // arrange
        string testedType = "yukata";
        var entities = new List<CatalogItemEntity>()
        {
            new CatalogItemEntity() { Name = ".NET Black & White Mug" }
        };
        var expectedResultDto = new List<CatalogItemDto>()
        {
            new CatalogItemDto() { Name = ".NET Black & White Mug" }
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i == testedType))).ReturnsAsync(entities);

        _mapper.Setup(s => s.Map(
            It.Is<List<CatalogItemEntity>>(i => i.Equals(entities)),
            typeof(List<CatalogItemEntity>),
            typeof(List<CatalogItemDto>))).Returns(expectedResultDto);

        // act
        var actualResult = await _catalogService.GetItemsByTypeAsync(testedType);

        // assert
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResultDto);
    }

    [Fact]
    public async Task GetItemsByTypeAsync_Failed()
    {
        // arrange
        string testedType = null!;

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i == testedType)))
            .ReturnsAsync((List<CatalogItemEntity>)null!);

        // act
        var actualResult = await _catalogService.GetItemsByTypeAsync(testedType);

        // assert
        actualResult.Should().BeNull();
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var entities = new List<CatalogBrandEntity>()
        {
            new CatalogBrandEntity() { Id = 1, Brand = "Azure" }
        };
        var expectedResultDto = new List<CatalogBrandDto>()
        {
            new CatalogBrandDto() { Id = 1, Brand = "Azure" }
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(entities);

        _mapper.Setup(s => s.Map(
            It.Is<List<CatalogBrandEntity>>(i => i.Equals(entities)),
            typeof(List<CatalogBrandEntity>),
            typeof(List<CatalogBrandDto>))).Returns(expectedResultDto);

        // actual
        var actualResult = await _catalogService.GetBrandsAsync();

        // assert
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResultDto);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetBrandsAsync())
            .ReturnsAsync((List<CatalogBrandEntity>)null!);

        // act
        var actualResult = await _catalogService.GetBrandsAsync();

        // assert
        actualResult.Should().BeNull();
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var entities = new List<CatalogTypeEntity>()
        {
            new CatalogTypeEntity() { Id = 13, Type = "yukata" }
        };
        var expectedResultDto = new List<CatalogTypeDto>()
        {
            new CatalogTypeDto() { Id = 13, Type = "yukata" }
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(entities);

        _mapper.Setup(s => s.Map(
            It.Is<List<CatalogTypeEntity>>(i => i.Equals(entities)),
            typeof(List<CatalogTypeEntity>),
            typeof(List<CatalogTypeDto>))).Returns(expectedResultDto);

        // act
        var actualResult = await _catalogService.GetTypesAsync();

        // assert
        actualResult.Should().NotBeNull();
        actualResult.Should().BeEquivalentTo(expectedResultDto);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetTypesAsync())
            .ReturnsAsync((List<CatalogTypeEntity>)null!);

        // act
        var actualResult = await _catalogService.GetTypesAsync();

        // assert
        actualResult.Should().BeNull();
    }
}