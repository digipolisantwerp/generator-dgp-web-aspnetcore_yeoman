using Microsoft.Framework.DependencyInjection;
using StarterKit.DataAccess;
using StarterKit.DataAccess.Repositories;
using StarterKit.DataAccess.Repositories.Base;
using StarterKit.DataAccess.Uow;

namespace StarterKit
{
	public class Factory
	{
		public static void Configure(IServiceCollection services)
		{
			ConfigureUtilitites(services);
			ConfigureServiceAgents(services);
			ConfigureDataAccess(services);
			ConfigureBusiness(services);
		}

		private static void ConfigureBusiness(IServiceCollection services)
		{
			services.AddScoped(typeof(IPager<>), typeof(Pager<>));
		}

		private static void ConfigureDataAccess(IServiceCollection services)
		{
			services.AddSingleton<IUowProvider, UowProvider>();
			services.AddTransient(typeof(IRepository<>), typeof(GenericEntityRepository<>));
        }

		private static void ConfigureServiceAgents(IServiceCollection services)
		{

        }

		private static void ConfigureUtilitites(IServiceCollection services)
		{

		}
	}
}