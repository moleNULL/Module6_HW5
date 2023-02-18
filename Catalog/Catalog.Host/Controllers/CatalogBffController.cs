using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests.GetItemByRequests;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Response.GetResponses;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Catalog.Host.Models.Enums;
using Catalog.Host.Configurations;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogService _catalogService;
    private readonly IOptions<CatalogConfig> _config;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService,
        IOptions<CatalogConfig> config)
    {
        _logger = logger;
        _catalogService = catalogService;
        _config = config;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCatalogItems(PaginatedItemsRequest<CatalogTypeFilter> request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex, request.Filters);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemByResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GetItemBadRequestResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetItemByIdAsync(GetItemByIdRequest request)
    {
        var result = await _catalogService.GetItemByIdAsync(request.Id);

        if (result is null)
        {
            return BadRequest(new GetItemBadRequestResponse<string>()
            {
                ResponseState = Enum.GetName(EntityModifyState.NotFound) !
            });
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemByResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GetItemBadRequestResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetItemByBrandAsync(GetItemByBrandRequest request)
    {
        var result = await _catalogService.GetItemsByBrandAsync(request.Brand);

        if (result is null)
        {
            return BadRequest(new GetItemBadRequestResponse<string>()
            {
                ResponseState = Enum.GetName(EntityModifyState.NotFound) !
            });
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetItemByResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(GetItemBadRequestResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetItemByTypeAsync(GetItemByTypeRequest request)
    {
        var result = await _catalogService.GetItemsByTypeAsync(request.Type);

        if (result is null)
        {
            return BadRequest(new GetItemBadRequestResponse<string>()
            {
                ResponseState = Enum.GetName(EntityModifyState.NotFound) !
            });
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetBrandsResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBrandsAsync()
    {
        var result = await _catalogService.GetBrandsAsync();

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetTypesResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypesAsync()
    {
        var result = await _catalogService.GetTypesAsync();

        return Ok(result);
    }
}