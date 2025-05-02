using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
	public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration) : IAuthenticationService
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
					Token = await CreateTokenAsync(user)
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
					Token = await CreateTokenAsync(user)
				};

			//Otherwise, Throw BadRequest Exception
			else
			{
				var errors = result.Errors.Select(E => E.Description).ToList();
				throw new BadRequestException(errors);
			}
		}

		private async Task<string> CreateTokenAsync(ApplicationUser user)
		{
			// Private Claims
			var Claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Email , user.Email!),
				new Claim(ClaimTypes.Name , user.UserName!),
				new Claim(ClaimTypes.NameIdentifier , user.Id!)
			};

			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
				Claims.Add(new Claim(ClaimTypes.Role, role));

			//Security Key And Algorithm
			var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];

			//Convert the secretKey from string to bytes to be a Security Key
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

			//Credintials => Security Key & Algorithm
			var credintials = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);

			//Create the JWT Token
			var token = new JwtSecurityToken(issuer: _configuration["JWTOptions:Issuer"],
											 audience: _configuration["JWTOptions:Audience"],
											 claims: Claims,
											 expires: DateTime.Now.AddHours(1),
											 signingCredentials: credintials);

			//Return the JWT Token as a string
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
