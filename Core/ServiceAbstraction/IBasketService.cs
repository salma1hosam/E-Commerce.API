using Shared.DataTransferObjects.BasketModule;

namespace ServiceAbstraction
{
	public interface IBasketService
	{
		Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket);
		Task<BasketDto> GetBasketAsync(string key);
		Task<bool> DeleteBasketAsync(string key);
	}
}
