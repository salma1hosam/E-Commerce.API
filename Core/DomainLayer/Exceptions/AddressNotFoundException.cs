
namespace DomainLayer.Exceptions
{
	public sealed class AddressNotFoundException(string userName) : NotFoundException($"User {userName} Has No Address")
	{
	}
}
