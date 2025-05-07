using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModule;

namespace Presentation.Controllers
{
	public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
	{
		[Authorize]
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken());
			return Ok(order);
		}
	}
}
