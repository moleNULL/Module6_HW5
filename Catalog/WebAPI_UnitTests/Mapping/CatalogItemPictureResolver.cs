using WebAPI_UnitTests.Configurations;
using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Models.Dtos;

namespace WebAPI_UnitTests.Mapping;

public class CatalogItemPictureResolver : IMemberValueResolver<CatalogItemEntity, CatalogItemDto, string, object>
{
    private readonly CatalogConfig _config;

    public CatalogItemPictureResolver(IOptionsSnapshot<CatalogConfig> config)
    {
        _config = config.Value;
    }

    public object Resolve(CatalogItemEntity source, CatalogItemDto destination, string sourceMember, object destMember, ResolutionContext context)
    {
        return $"{_config.Host}/{_config.ImgUrl}/{sourceMember}";
    }
}