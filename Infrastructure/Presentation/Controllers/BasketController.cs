using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;

namespace Presentation.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class BasketController(IServiceManager _serviceManager) : ControllerBase
	{
		[HttpGet] //GET : BaseUrl/api/Basket
		public async Task<ActionResult<BasketDto>> GetBasket(string Key)
		{
			var Basket = await _serviceManager.BasketService.GetBasketAsync(Key);
			return Ok(Basket);
		}

		[HttpPost]
		public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
		{
			var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
			return Ok(Basket);
		}

		[HttpDelete("{Key}")] //DELETE : BaseUrl/api/Basket/fsnvksnkmcw
		public async Task<ActionResult<bool>> DeleteBasket(string Key)
		{
			var Result = await _serviceManager.BasketService.DeleteBasketAsync(Key);
			return Ok(Result);
		}
	}
}
