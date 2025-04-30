
namespace DomainLayer.Exceptions
{
	public sealed class ProductNotFoundException(int id) : NotFoundException($"Product With Id = {id} Is Not Found")
	{
	}
}
