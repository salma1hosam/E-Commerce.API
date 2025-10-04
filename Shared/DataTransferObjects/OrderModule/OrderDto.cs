using Shared.DataTransferObjects.IdentityModule;

namespace Shared.DataTransferObjects.OrderModule
{
	public class OrderDto
	{
		public string BasketId { get; set; } = default!;
		public AddressDto ShipToAddress { get; set; } = default!;
		public int DeliveryMethodId { get; set; }
	}
}
