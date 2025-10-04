using AutoMapper;
using DomainLayer.Models.BasketModule;
using Shared.DataTransferObjects.BasketModule;

namespace Service.MappingProfiles
{
	public class BasketProfile : Profile
	{
		public BasketProfile()
		{
			CreateMap<CustomerBasket, BasketDto>().ReverseMap();
			CreateMap<BasketItem, BasketItemDto>().ReverseMap();
		}
	}
}
