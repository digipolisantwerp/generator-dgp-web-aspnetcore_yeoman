using Digipolis.Errors;
using FOOBAR.Shared.Exceptions.Handler;
using FOOBAR.Shared.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FOOBAR.Shared.Extensions
{
	public static class ApiExtensions
	{
		public static IServiceCollection AddGlobalErrorHandling<TExceptionMapper>(this IServiceCollection services)
			where TExceptionMapper : ExceptionMapper
		{
			services.TryAddSingleton<IExceptionMapper, TExceptionMapper>();
			services.TryAddSingleton<IExceptionHandler, ExceptionHandler>();

			return services;
		}

		public static void UseApiExtensions(this IApplicationBuilder app)
		{
			var settings = app.ApplicationServices.GetService<IOptions<AppSettings>>();

			if (settings?.Value?.DisableGlobalErrorHandling == false)
			{
				app.UseExceptionHandler(new ExceptionHandlerOptions
				{
					ExceptionHandler = new ExceptionResponseHandler().Invoke
				});
			}

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				RequireHeaderSymmetry = true,
				ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedHost |
				                   Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
			});
		}
	}
}
