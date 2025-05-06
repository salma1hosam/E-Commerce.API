using AutoMapper;
using DomainLayer.Models.OrderModule;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;

namespace Service.MappingProfiles
{
	internal class OrderProfile : Profile
	{
		public OrderProfile()
		{
			CreateMap<AddressDto, OrderAddress>();
		}
	}
}
