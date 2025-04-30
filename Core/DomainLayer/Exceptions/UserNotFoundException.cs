
namespace DomainLayer.Exceptions
{
	public sealed class UserNotFoundException(string email) : NotFoundException($"User With Email {email} Is Not Found")
	{
	}
}
