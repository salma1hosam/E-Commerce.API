
namespace DomainLayer.Models.BasketModule
{
	//This Model Will Not be Represented as a Table in the Database so, It Will Not inherit from the BaseEntity Class
	public class BasketItem
	{
		public int Id { get; set; }
		public string ProductName { get; set; } = default!;
		public string PictureUrl { get; set; } = default!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
