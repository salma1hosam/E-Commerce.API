using Microsoft.Extensions.Configuration;

namespace Persistence
{
	public static class InfrastructureServicesRegisteration
	{
		//Extension Method To Add all the Persistence Layer (Infrastructur) Services to the Services Container through it
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
		{
			Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			Services.AddScoped<IDataSeeding, DataSeeding>();
			Services.AddScoped<IUnitOfWork, UnitOfWork>();
			return Services;
		}
	}
}
