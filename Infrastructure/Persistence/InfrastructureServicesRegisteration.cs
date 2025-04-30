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

			//Adding or Registering the Redis Connection Service
			Services.AddSingleton<IConnectionMultiplexer>((_) =>
			{
				return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnectionString"));
			});

			//Adding or Registering the Identity DbContext Services
			Services.AddDbContext<StoreIdentityDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
			});

			return Services;
		}
	}
}
