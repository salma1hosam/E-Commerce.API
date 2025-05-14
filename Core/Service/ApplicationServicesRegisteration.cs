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
			Services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

			Services.AddScoped<IProductService, ProductService>();
			Services.AddScoped<Func<IProductService>>(provider =>
													  () => provider.GetRequiredService<IProductService>());

			Services.AddScoped<IBasketService, BasketService>();
			Services.AddScoped<Func<IBasketService>>(provider =>
													 () => provider.GetRequiredService<IBasketService>());

			Services.AddScoped<IAuthenticationService, AuthenticationService>();
			Services.AddScoped<Func<IAuthenticationService>>(provider =>
															 () => provider.GetRequiredService<IAuthenticationService>());

			Services.AddScoped<IOrderService, OrderService>();
			Services.AddScoped<Func<IOrderService>>(provider =>
													() => provider.GetRequiredService<IOrderService>());

			return Services;
		}
	}
}
