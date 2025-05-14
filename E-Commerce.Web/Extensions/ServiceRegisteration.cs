using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace E_Commerce.Web.Extensions
{
	public static class ServiceRegisteration
	{
		//Extension Method To Add all the Swagger Services to the Services Container through it
		public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			Services.AddEndpointsApiExplorer();
			Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					Description = "Enter 'Bearer' Followed By Space And Your Token"
				});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							}
						},
						new string[] { }  //Scope
					}
				});
			});

			return Services;
		}

		//Extension Method To Add all the WebApplication Layer Services to the Services Container through it
		public static IServiceCollection AddWebApplicationServices(this IServiceCollection Services)
		{
			Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorsResponse;  //Passing the Function itself (its address) as a delegate [Not calling the function]
			});
			return Services;
		}

		public static IServiceCollection AddJWTServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			Services.AddAuthentication(config =>
			{
				config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //The Schema that will be compared to when retrieving
			}).AddJwtBearer(options =>
			{
				////Save the JWT Token in the http context
				//options.SaveToken = true;

				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = Configuration["JWTOptions:Issuer"],

					ValidateAudience = true,
					ValidAudience = Configuration["JWTOptions:Audience"],

					ValidateLifetime = true,

					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTOptions:SecretKey"]))
				};
			});
			return Services;
		}
	}
}
