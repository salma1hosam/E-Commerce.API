using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
	public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
	{
		[HttpPost("Login")] //POST BaseUrl/api/Authentication/Login
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
			return Ok(user);
		}

		[HttpPost("Register")] //POST BaseUrl/api/Authentication/Register
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			var user = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
			return Ok(user);
		}

		[HttpGet("CheckEmail")]
		public async Task<ActionResult<bool>> CheckEmail(string email)
		{
			var result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
			return Ok(result);
		}

		[Authorize]
		[HttpGet("CurrentUser")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var appUser = await _serviceManager.AuthenticationService.GetCurrentUserAsync(email!);
			return Ok(appUser);
		}

		[Authorize]
		[HttpGet("Address")]
		public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var address = await _serviceManager.AuthenticationService.GetCurrentUserAddressAsync(email!);
			return Ok(address);
		}

		[Authorize]
		[HttpPut("Address")]
		public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto addressDto)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var updatedAddress = await _serviceManager.AuthenticationService.UpdateCurrentUserAddressAsync(email, addressDto);
			return Ok(updatedAddress);
		}
	}
}
