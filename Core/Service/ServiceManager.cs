using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstraction;

namespace Service
{
	public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IServiceManager
	{
		private readonly Lazy<IProductService> _lazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOfWork, _mapper));
		public IProductService ProductService => _lazyProductService.Value;

		private readonly Lazy<IBasketService> _lazyBasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
		public IBasketService BasketService => _lazyBasketService.Value;
	}
}
