using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;

namespace Service
{
	public static class ApplicationServicesRegisteration
	{
		//Extension Method To Add all the Service Layer Services to the Services Container through it
		public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
		{
			Services.AddAutoMapper(typeof(Service.AssemblyReference).Assembly);
			Services.AddScoped<IServiceManager, ServiceManager>();
			return Services;
		}
	}
}
