using Microsoft.Extensions.DependencyInjection;

namespace FOOBAR
{
	public static class DependencyRegistration
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			// Register your own services here, e.g. services.AddTransient<IMyService, MyService>();
			return services;
		}

		public static IServiceCollection AddHelperServices(this IServiceCollection services)
		{
			return services;
		}
	}
}
