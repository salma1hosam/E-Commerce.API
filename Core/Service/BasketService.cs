using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;

namespace Service
{
	public class BasketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketService
	{
		public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
		{
			var customerBasket = _mapper.Map<BasketDto, CustomerBasket>(basket);
			var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);
			if (CreatedOrUpdatedBasket is not null)
				return await GetBasketAsync(basket.Id);
			else
				throw new Exception("Can Not Create Or Update The Basket Now , Try Again Later");
		}

		public async Task<bool> DeleteBasketAsync(string key) => await _basketRepository.DeleteBasketAsync(key);

		public async Task<BasketDto> GetBasketAsync(string key)
		{
			var basket = await _basketRepository.GetBasketAsync(key);
			if (basket is not null)
				return _mapper.Map<CustomerBasket, BasketDto>(basket);
			else
				throw new BasketNotFoundException(key);
		}
	}
}
