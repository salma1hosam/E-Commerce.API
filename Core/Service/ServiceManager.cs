using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;

namespace Service
{
	public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper,
								IBasketRepository _basketRepository,
								UserManager<ApplicationUser> _userManager,
								IConfiguration _configuration)
	{
		private readonly Lazy<IProductService> _lazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
		public IProductService ProductService => _lazyProductService.Value;


		private readonly Lazy<IBasketService> _lazyBasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
		public IBasketService BasketService => _lazyBasketService.Value;


		private readonly Lazy<IAuthenticationService> _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager, _configuration, _mapper));
		public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;


		private readonly Lazy<IOrderService> _lazyOrderService = new Lazy<IOrderService>(() => new OrderService(_unitOfWork, _mapper, _basketRepository));
		public IOrderService OrderService => _lazyOrderService.Value;
	}
}
