using AutoMapper;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.ProductModule;

namespace Service.MappingProfiles
{
    public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductDto>()
				.ForMember(dist => dist.ProductBrand, options => options.MapFrom(src => src.ProductBrand.Name))
				.ForMember(dist => dist.ProductType, options => options.MapFrom(src => src.ProductType.Name))
				.ForMember(dist => dist.PictureUrl, options => options.MapFrom<PictureUrlResolver>());

			CreateMap<ProductType, TypeDto>();
			CreateMap<ProductBrand, BrandDto>();
		}
	}
}
