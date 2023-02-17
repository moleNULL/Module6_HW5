using WebAPI_UnitTests.Data.Entities;
using WebAPI_UnitTests.Models.Dtos;

namespace WebAPI_UnitTests.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogItemEntity, CatalogItemDto>()
            .ForMember("PictureUrl", opt
                => opt.MapFrom<CatalogItemPictureResolver, string>(c => c.PictureFileName));
        CreateMap<CatalogBrandEntity, CatalogBrandDto>();
        CreateMap<CatalogTypeEntity, CatalogTypeDto>();
    }
}