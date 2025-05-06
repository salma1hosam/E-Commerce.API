using Shared.DataTransferObjects.OrderModule;

namespace ServiceAbstraction
{
	public interface IOrderService
	{
		Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email);
	}
}
