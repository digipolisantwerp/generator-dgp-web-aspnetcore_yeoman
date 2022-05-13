using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FOOBAR.Shared.Exceptions.Handler
{
	public interface IExceptionHandler
	{
		Task HandleAsync(HttpContext context, Exception ex);
		void Handle(HttpContext context, Exception ex);
	}
}
