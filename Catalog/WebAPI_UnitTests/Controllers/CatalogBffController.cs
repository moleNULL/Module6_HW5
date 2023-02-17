using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Models.Dtos;
using WebAPI_UnitTests.Models.Requests.GetItemByRequests;
using WebAPI_UnitTests.Models.Requests;
using WebAPI_UnitTests.Models.Response.GetResponses;
using WebAPI_UnitTests.Models.Response;
using WebAPI_UnitTests.Services.Interfaces;

namespace WebAPI_UnitTests.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetCatalogItemsAsync(PaginatedItemsRequest request)
    {
        var result = await _catalogService.GetCatalogItemsAsync(request.PageSize, request.PageIndex);

        if (result is null)
        {
            return BadRequest(result);
        }

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