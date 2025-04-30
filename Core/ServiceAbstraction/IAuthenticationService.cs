using Shared.DataTransferObjects.IdentityModule;

namespace ServiceAbstraction
{
	public interface IAuthenticationService
	{
		Task<UserDto> LoginAsync(LoginDto loginDto);
		Task<UserDto> RegisterAsync(RegisterDto registerDto);
	}
}
