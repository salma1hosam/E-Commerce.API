
namespace DomainLayer.Exceptions
{
	public sealed class OrderNotFoundException(Guid id) : NotFoundException($"Order With Id = {id} is Not Found")
	{
	}
}
