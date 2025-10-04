using Shared.DataTransferObjects.IdentityModule;

namespace Shared.DataTransferObjects.OrderModule
{
	public class OrderToReturnDto
	{
		public Guid Id { get; set; }
		public string BuyerEmail { get; set; } = default!;
		public DateTimeOffset OrderDate { get; set; }
		public ICollection<OrderItemDto> Items { get; set; } = [];
		public AddressDto ShipToAddress { get; set; } = default!;
		public string DeliveryMethod { get; set; } = default!;
        public decimal DeliveryCost { get; set; }
        public string Status { get; set; } = default!;
		public decimal SubTotal { get; set; }
		public decimal Total { get; set; }
	}
}
