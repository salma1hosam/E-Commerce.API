using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Persistence.Identity;
using StackExchange.Redis;

namespace Persistence
{
	public static class InfrastructureServicesRegisteration
	{
		//Extension Method To Add all the Persistence Layer (Infrastructur) Services to the Services Container through it
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			//Adding or Registering the DbContext Service
			Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			Services.AddScoped<IDataSeeding, DataSeeding>();
			Services.AddScoped<IUnitOfWork, UnitOfWork>();
			Services.AddScoped<IBasketRepository, BasketRepository>();
			Services.AddScoped<ICacheRepository, CacheRepository>();

			//Adding or Registering the Redis Connection Service
			Services.AddSingleton<IConnectionMultiplexer>((_) =>
			{
				return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnectionString"));
			});

			//Adding or Registering the Identity DbContext Service
			Services.AddDbContext<StoreIdentityDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
			});

			//Adding or Registering the Identity Services  (Anything related to Cookie Authentication or Token Providers are not added by default)
			Services.AddIdentityCore<ApplicationUser>()
					.AddRoles<IdentityRole>()       //Not Add By Default
					.AddEntityFrameworkStores<StoreIdentityDbContext>();

			return Services;
		}
	}
}
