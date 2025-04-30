using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddlewares;

namespace E_Commerce.Web.Extensions
{
	public static class WebApplicationRegisteration
	{
		public static async Task SeedDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var objectOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
			await objectOfDataSeeding.DataSeedAsync();
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
			app.UseSwaggerUI();
			return app;
		}
	}
}
