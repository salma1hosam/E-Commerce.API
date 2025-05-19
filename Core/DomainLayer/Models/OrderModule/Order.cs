using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models.OrderModule
{
	public class Order : BaseEntity<Guid>
	{
		public Order() //For EF Core to Generate Migration
		{

		}
		public Order(string userEmail, OrderAddress address, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
		{
			BuyerEmail = userEmail;
			ShipToAddress = address;
			DeliveryMethod = deliveryMethod;
			Items = items;
			SubTotal = subTotal;
		}

		public string BuyerEmail { get; set; } = default!;
		public OrderAddress ShipToAddress { get; set; } = default!;
		public DeliveryMethod DeliveryMethod { get; set; } = default!;
		public ICollection<OrderItem> Items { get; set; } = []; //Empty List
		public decimal SubTotal { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public OrderStatus Status { get; set; }
		public int DeliveryMethodId { get; set; } //FK

		//[NotMapped]
		//public decimal Total { get => SubTotal + DeliveryMethod.Price; }

		//OR
		//Methods will not be represented in DB ,so EF Core will Ignore it. (Must be Named 'GetX' For Mapping)
		public decimal GetTotal() => SubTotal + DeliveryMethod.Price;
	}
}
