using Shared.DataTransferObjects.IdentityModule;

namespace ServiceAbstraction
{
	public interface IAuthenticationService
	{
		Task<UserDto> LoginAsync(LoginDto loginDto);
		Task<UserDto> RegisterAsync(RegisterDto registerDto);
		Task<bool> CheckEmailAsync(string email);
		Task<AddressDto> GetCurrentUserAddressAsync(string email);
		Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto);
		Task<UserDto> GetCurrentUserAsync(string email);
	}
}
