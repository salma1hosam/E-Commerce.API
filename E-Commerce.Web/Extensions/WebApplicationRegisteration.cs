using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

namespace E_Commerce.Web.Extensions
{
	public static class WebApplicationRegisteration
	{
		public static async Task SeedDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var objectOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

			await objectOfDataSeeding.DataSeedAsync();
			await objectOfDataSeeding.IdentityDataSeedAsync();
		}

		//Adding the Middlewares (all the related middlewares in a method) through the Extension Methods

		public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<CustomExceptionHandlerMiddleware>();
			return app;
		}

		public static IApplicationBuilder UseSwaggerMiddlewares(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.ConfigObject = new ConfigObject()
				{
					DisplayRequestDuration = true
				};

				//Title
				options.DocumentTitle = "My E-Commerce API";

				////To Change the default Swagger UI Design with another design you provide
				//options.IndexStream =

				////Change the Json Serializing of the request and response
				//options.JsonSerializerOptions = new JsonSerializerOptions()
				//{
				//	PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				//};

				options.DocExpansion(DocExpansion.None);

				options.EnableFilter();

				//The UI Of Authorization
				options.EnablePersistAuthorization();
			});
			return app;
		}
	}
}
