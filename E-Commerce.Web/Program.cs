using E_Commerce.Web.Extensions;
using Persistence;
using Service;

namespace E_Commerce.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Add services to the container.

			builder.Services.AddControllers();

			//Registering all the Swagger Services through this Extension Method
			builder.Services.AddSwaggerServices();

			//Registering all the Services of the Persistence Layer through this Extension Method
			builder.Services.AddInfrastructureServices(builder.Configuration);

			//Registering all the Services of the Service Implementation Layer through this Extension Method
			builder.Services.AddApplicationServices();

			//Registering all the Services related to this layer (WebApplication Layer) through this Extension Method
			builder.Services.AddWebApplicationServices();
			#endregion

			var app = builder.Build();

			//Seeding Data in Database through this Extension Method for the WebApplication
			await app.SeedDatabaseAsync();

			#region Configure the HTTP request pipeline.(Middlewares)

			//Exception Middlewares Must be at Beginning of the Middlewares
			app.UseCustomExceptionMiddleware(); //Adding the Custom Exception Middleware in the container through the IApplicationBuilder(WebApplication Inherit from it) Extention Method

			if (app.Environment.IsDevelopment())
				app.UseSwaggerMiddlewares();  //Adding Swagger Middlewares through the Extension Method

			app.UseHttpsRedirection();
			app.UseStaticFiles();   //Routing to the static files (to wwwroot)

			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}
