using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;

namespace Service
{
	public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
	{
		public async Task<UserDto> LoginAsync(LoginDto loginDto)
		{
			//Check if the Email Exists
			var user = await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email);

			//Check if the Password is Correct
			var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			//Return UserDto
			if (isPasswordValid)
				return new UserDto()
				{
					Email = user.Email,
					DisplayName = user.DisplayName,
					Token = CreateTokenAsync(user)
				};
			else
				throw new UnauthorizedException();
		}


		public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
		{
			//Map from RegisterDto => ApplicationUser
			var user = new ApplicationUser()
			{
				Email = registerDto.Email,
				DisplayName = registerDto.DisplayName,
				UserName = registerDto.UserName,
				PhoneNumber = registerDto.PhoneNumber
			};

			//Create User [ApplicationUser]
			var result = await _userManager.CreateAsync(user, registerDto.Password);

			//if created successfully, Return UserDto
			if (result.Succeeded)
				return new UserDto()
				{
					Email = user.Email,
					DisplayName = user.DisplayName,
					Token = CreateTokenAsync(user)
				};

			//Otherwise, Throw BadRequest Exception
			else
			{
				var errors = result.Errors.Select(E => E.Description).ToList();
				throw new BadRequestException(errors);
			}
		}

		private string CreateTokenAsync(ApplicationUser user)
		{
			return "Token_TODO";
		}
	}
}
