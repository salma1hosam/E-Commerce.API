using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
	public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
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
			var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//Create the JWT Token
			var token = new JwtSecurityToken(issuer: _configuration["JWTOptions:Issuer"],
											 audience: _configuration["JWTOptions:Audience"],
											 claims: Claims,
											 expires: DateTime.Now.AddHours(1),
											 signingCredentials: credintials);

			//Return the JWT Token as a string
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<bool> CheckEmailAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			return user is not null;
		}

		public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
		{
			var user = await _userManager.Users.Include(U => U.Address)
											   .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException(email);
			if (user.Address is not null)
				return _mapper.Map<Address, AddressDto>(user.Address);
			else
				throw new AddressNotFoundException(user.UserName);
		}

		public async Task<UserDto> GetCurrentUserAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
			return new UserDto()
			{
				Email = user.Email,
				DisplayName = user.DisplayName,
				Token = await CreateTokenAsync(user)
			};
		}
		public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
		{
			var user = await _userManager.Users.Include(U => U.Address)
											   .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException(email);
			if (user.Address is not null)  //Update Address
			{
				user.Address.FirstName = addressDto.FirstName;
				user.Address.LastName = addressDto.LastName;
				user.Address.City = addressDto.City;
				user.Address.Country = addressDto.Country;
				user.Address.Street = addressDto.Street;
			}
			else //Add New Address
				user.Address = _mapper.Map<AddressDto, Address>(addressDto);

			await _userManager.UpdateAsync(user);

			return _mapper.Map<AddressDto>(user.Address);
		}

	}
}
