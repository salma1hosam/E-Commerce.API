using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Extensions
{
	public static class ServiceRegisteration
	{
		//Extension Method To Add all the Swagger Services to the Services Container through it
		public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			Services.AddEndpointsApiExplorer();
			Services.AddSwaggerGen();
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
	}
}
