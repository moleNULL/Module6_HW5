using WebAPI_UnitTests.Models.Response.ItemResponses;
using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Models.Requests.ItemRequests;
using WebAPI_UnitTests.Services.Interfaces;

namespace WebAPI_UnitTests.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogItemController : ControllerBase
{
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(
        ILogger<CatalogItemController> logger,
        ICatalogItemService catalogItemService)
    {
        _catalogItemService = catalogItemService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddAsync(CreateProductRequest request)
    {
        var result = await _catalogItemService.AddAsync(request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrandId, request.CatalogTypeId, request.PictureFileName);

        if (result is null)
        {
            return BadRequest(new AddItemResponse<int?>() { Id = result });
        }

        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(RemoveItemResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RemoveItemResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveAsync(RemoveProductRequest request)
    {
        var result = await _catalogItemService.RemoveAsync(request.Id);

        if (result == EntityModifyState.NotFound)
        {
            return BadRequest(new RemoveItemResponse<string>() { RemoveState = Enum.GetName(result) ! });
        }

        return Ok(new RemoveItemResponse<string>() { RemoveState = Enum.GetName(result) ! });
    }

    [HttpPost]
    [ProducesResponseType(typeof(EntityModifyState), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest request)
    {
        var result = await _catalogItemService.UpdateAsync(request.Id, request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrandId, request.CatalogTypeId, request.PictureFileName);

        if (result == EntityModifyState.NotFound || result == EntityModifyState.NotUpdated)
        {
            return BadRequest(new UpdateItemResponse<string>() { UpdateState = Enum.GetName(result) ! });
        }

        return Ok(new UpdateItemResponse<string>() { UpdateState = Enum.GetName(result) ! });
    }
}