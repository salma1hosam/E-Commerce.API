using AutoMapper;
using DomainLayer.Models.IdentityModule;
using Shared.DataTransferObjects.IdentityModule;

namespace Service.MappingProfiles
{
	public class IdentityProfile : Profile
	{
        public IdentityProfile()
        {
            CreateMap<Address , AddressDto>().ReverseMap();
        }
    }
}
