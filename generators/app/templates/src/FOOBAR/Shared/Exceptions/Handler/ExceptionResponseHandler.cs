using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FOOBAR.Shared.Exceptions.Handler
{
	public class ExceptionResponseHandler
	{
		public async Task Invoke(HttpContext context)
		{
			var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
			var handler = context.RequestServices.GetService<IExceptionHandler>();

			if (handler == null || exceptionDetails?.Error == null)
				return;

			await handler.HandleAsync(context, exceptionDetails.Error);
		}
	}
}
