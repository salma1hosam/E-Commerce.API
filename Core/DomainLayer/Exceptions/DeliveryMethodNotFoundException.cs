
namespace DomainLayer.Exceptions
{
	public sealed class DeliveryMethodNotFoundException(int id) : NotFoundException($"No Delivery Method With Id = {id}")
	{
	}
}
