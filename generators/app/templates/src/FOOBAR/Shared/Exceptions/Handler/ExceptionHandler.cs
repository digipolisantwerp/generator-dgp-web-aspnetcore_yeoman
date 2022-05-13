using System;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Errors;
using FOOBAR.Shared.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FOOBAR.Shared.Exceptions.Handler
{
	// Pulled out of Digipolis.Web package untill updated to .net core 3.1
	// https://github.com/digipolisantwerp/web_aspnetcore/blob/master/src/Digipolis.Web/Exceptions/ExceptionHandler.cs

	public class ExceptionHandler : IExceptionHandler
	{
		private readonly AppSettings _appSettings;
		private readonly ILogger<ExceptionHandler> _logger;
		private readonly IExceptionMapper _mapper;
		private readonly IOptions<MvcNewtonsoftJsonOptions> _options;

		public ExceptionHandler(IExceptionMapper mapper, ILogger<ExceptionHandler> logger,
			IOptions<MvcNewtonsoftJsonOptions> options, IOptions<AppSettings> appSettings)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_options = options ?? throw new ArgumentNullException(nameof(options));
			_appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
		}

		public async Task HandleAsync(HttpContext context, Exception ex)
		{
			var error = _mapper?.Resolve(ex);

			if (error == null)
				return;

			context.Response.StatusCode = error.Status != 0 ? error.Status : context.Response.StatusCode;

			if (HasMeaningFullProperty(error))
			{
				context.Response.ContentType = "application/problem+json";

				var json = JsonConvert.SerializeObject(error,
					_options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
				await context.Response.WriteAsync(json);
			}

			LogException(error, ex);
		}

		public void Handle(HttpContext context, Exception ex)
		{
			var error = _mapper?.Resolve(ex);

			if (error == null)
				return;

			if (HasMeaningFullProperty(error))
			{
				context.Response.Clear();
				context.Response.ContentType = "application/problem+json";

				var json = JsonConvert.SerializeObject(error,
					_options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
				context.Response.WriteAsync(json).Wait();
			}

			context.Response.StatusCode = error.Status != 0 ? error.Status : context.Response.StatusCode;

			LogException(error, ex);
		}

		private void LogException(Error error, Exception exception)
		{
			var logMessage = new ExceptionLogMessage
			{
				Error = error,
				ExceptionInfo = exception.ToString(),
				Exception = exception
			};


			var logAsJson = JsonConvert.SerializeObject(logMessage,
				_options?.Value?.SerializerSettings ?? new JsonSerializerSettings());
			if (error.Status >= 500 && error.Status <= 599)
				_logger?.LogError(logAsJson);
			else if (error.Status >= 400 && error.Status <= 499)
				_logger?.LogDebug(logAsJson);
			else
				_logger?.LogInformation(logAsJson);
		}

		private static bool HasMeaningFullProperty(Error error)
		{
			return !string.IsNullOrWhiteSpace(error.Title) || !string.IsNullOrWhiteSpace(error.Code) ||
			       error.Type != null || error.ExtraInfo?.Any() == true;
		}
	}
}
