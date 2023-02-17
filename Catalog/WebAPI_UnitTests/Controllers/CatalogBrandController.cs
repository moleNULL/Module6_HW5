using WebAPI_UnitTests.Data;
using WebAPI_UnitTests.Models.Requests.BrandRequests;
using WebAPI_UnitTests.Models.Response.BrandResponses;
using WebAPI_UnitTests.Services.Interfaces;

namespace WebAPI_UnitTests.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(ILogger<CatalogBrandController> logger, ICatalogBrandService catalogBrandService)
    {
        _logger = logger;
        _catalogBrandService = catalogBrandService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(AddBrandResponse<int?>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddAsync(CreateBrandRequest request)
    {
        var result = await _catalogBrandService.AddAsync(request.Brand);

        if (result is null)
        {
            return BadRequest(new AddBrandResponse<int?>() { Id = result });
        }

        return Ok(new AddBrandResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType(typeof(RemoveBrandResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(RemoveBrandResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveAsync(RemoveBrandRequest request)
    {
        var result = await _catalogBrandService.RemoveAsync(request.Id);

        if (result == EntityModifyState.NotFound)
        {
            return BadRequest(new RemoveBrandResponse<string>() { RemoveState = Enum.GetName(result) ! });
        }

        return Ok(new RemoveBrandResponse<string>() { RemoveState = Enum.GetName(result) ! });
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateBrandResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(UpdateBrandResponse<string>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateBrandRequest request)
    {
        var result = await _catalogBrandService.UpdateAsync(request.Id, request.Brand);

        if (result == EntityModifyState.NotFound || result == EntityModifyState.NotUpdated)
        {
            return BadRequest(new UpdateBrandResponse<string>() { UpdateState = Enum.GetName(result) ! });
        }

        return Ok(new UpdateBrandResponse<string>() { UpdateState = Enum.GetName(result) ! });
    }
}