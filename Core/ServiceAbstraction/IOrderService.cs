using Shared.DataTransferObjects.OrderModule;

namespace ServiceAbstraction
{
	public interface IOrderService
	{
		Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email);
		Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
		Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email);
		Task<OrderToReturnDto> GetOrderByIdAsync(Guid id);
	}
}
