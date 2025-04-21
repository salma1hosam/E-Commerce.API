using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using ServiceAbstraction;

namespace E_Commerce.Web
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Add services to the container.

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddScoped<IDataSeeding, DataSeeding>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddAutoMapper(typeof(Service.AssemblyReference).Assembly);
			builder.Services.AddScoped<IServiceManager, ServiceManager>();
			#endregion

			var app = builder.Build();

			#region Data Seeding
			using var scope = app.Services.CreateScope();
			var objectOfDataSeeding = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
			await objectOfDataSeeding.DataSeedAsync();
			#endregion

			#region Configure the HTTP request pipeline.(Middlewares)
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();   //Routing to the static files (to wwwroot)

			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}
