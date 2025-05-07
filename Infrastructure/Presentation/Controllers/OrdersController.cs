using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.OrderModule;

namespace Presentation.Controllers
{
	[Authorize]
	public class OrdersController(IServiceManager _serviceManager) : ApiBaseController
	{
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken());
			return Ok(order);
		}

		[AllowAnonymous]
		[HttpGet("DeliveryMethods")]
		public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethods);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
		{
			var orders = await _serviceManager.OrderService.GetAllOrdersAsync(GetEmailFromToken());
			return Ok(orders);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id)
		{
			var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
			return Ok(order);
		}
	}
}
