using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models.OrderModule
{
	public class Order : BaseEntity<Guid>
	{
		public string UserEmail { get; set; } = default!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public OrderStatus Status { get; set; }
		public OrderAddress Address { get; set; } = default!;
		public DeliveryMethod DeliveryMethod { get; set; } = default!;
		public int DeliveryMethodId { get; set; } //FK
		public ICollection<OrderItem> Items { get; set; } = []; //Empty List
		public decimal SubTotal { get; set; }

		//[NotMapped]
		//public decimal Total { get => SubTotal + DeliveryMethod.Price; }

		//OR
		//Methods will not be represented in DB ,so EF Core will Ignore it. (Must be Named 'GetX' For Mapping)
		public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
	}
}
