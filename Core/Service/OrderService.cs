using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications.OrderModuleSpecifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;

namespace Service
{
	public class OrderService(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository) : IOrderService
	{
		public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
		{
			//Map AddressDto to OrderAddress
			var orderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

			//Get the Basket from the BasketId
			var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);

			//Create OrderItem List
			List<OrderItem> orderItems = [];
			var productRepo = _unitOfWork.GetRepository<Product, int>();
			foreach (var item in basket.Items)
			{
				var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
				var orderItem = new OrderItem()
				{
					Product = new ProductItemOrdered()
					{
						ProductId = product.Id,
						ProductName = product.Name,
						PictureUrl = product.PictureUrl
					},
					Price = product.Price,
					Quantity = item.Quantity
				};
				orderItems.Add(orderItem);
			}

			//Get the DeliveryMethod
			var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId)
				?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

			//Calculate the SubTotal
			var subTotal = orderItems.Sum(item => item.Quantity * item.Price);

			//Create the Order Object
			var order = new Order(email, orderAddress, deliveryMethod, orderItems, subTotal);

			//Add the Order
			await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
			var rows = await _unitOfWork.SaveChangesAsync();
			if (rows < 1) throw new Exception("Order Is Not Created");

			//Map From Order => OrderToReturnDto
			return _mapper.Map<Order, OrderToReturnDto>(order);
		}

		public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
		{
			var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecification(email));
			return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
		}

		public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
		{
			var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
			return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
		}

		public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
		{
			var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecification(id))
				?? throw new OrderNotFoundException(id);
			return _mapper.Map<Order, OrderToReturnDto>(order);
		}
	}
}
