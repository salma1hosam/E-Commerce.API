
namespace DomainLayer.Models.BasketModule
{
	//This Model Will Not be Represented as a Table in the Database so, It Will Not inherit from the BaseEntity Class
	public class CustomerBasket
	{
		public string Id { get; set; } //GUID : Created From the Client [Front-end]
		public ICollection<BasketItem> Items { get; set; } = [];
	}
}
