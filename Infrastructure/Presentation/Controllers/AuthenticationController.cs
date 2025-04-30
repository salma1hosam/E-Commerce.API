using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;

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
	}
}
