using System;
using System.Net;
using Digipolis.Errors;
using Digipolis.Errors.Exceptions;
using FOOBAR.Shared.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FOOBAR.Startup
{
	public class ApiExceptionMapper : ExceptionMapper
	{
		private readonly IWebHostEnvironment _environment;

		public ApiExceptionMapper(ILogger<ApiExceptionMapper> logger, IWebHostEnvironment environment)
		{
			Logger = logger ??
			         throw new ArgumentException($"{GetType().Name}.Ctr parameter {nameof(logger)} cannot be null.");

			_environment = environment ??
			               throw new ArgumentException(
				               $"{GetType().Name}.Ctr parameter {nameof(environment)} cannot be null.");
		}

		protected ILogger<ApiExceptionMapper> Logger { get; private set; }

		protected override void Configure()
		{
			CreateMap<UnauthorizedAccessException>((error, exception) =>
			{
				CreateUnauthorizedMap(error, new UnauthorizedException(exception: exception));
			});
			CreateMap<System.ComponentModel.DataAnnotations.ValidationException>((error, exception) =>
			{
				CreateValidationMap(error, new ValidationException(exception: exception));
			});
		}

		protected override void CreateNotFoundMap(Error error, NotFoundException exception)
		{
			base.CreateNotFoundMap(error, exception);
			Logger.LogWarning($"Not found: {exception.Message}", exception);
		}

		protected override void CreateUnauthorizedMap(Error error, UnauthorizedException exception)
		{
			base.CreateUnauthorizedMap(error, exception);
			Logger.LogWarning($"Access denied: {exception.Message}", exception);
		}

		protected override void CreateDefaultMap(Error error, Exception exception)
		{
			if (_environment.IsDevelopment())
			{
				error.Status = (int)HttpStatusCode.InternalServerError;
				error.Title = $"{exception.GetType().Name}: {exception.Message}";
				AddInnerExceptions(error, exception);
			}
			else
			{
				error.Status = (int)HttpStatusCode.InternalServerError;
				error.Title = "We are experiencing some technical difficulties.";
			}

			Logger.LogError("Internal server error: {exceptionMessage} {exception}", exception.Message, exception);
		}

		protected override void CreateValidationMap(Error error, ValidationException exception)
		{
			error.ExtraInfo = exception.Messages;
			base.CreateValidationMap(error, exception);
			error.Title = exception.Message;
			Logger.LogWarning($"Validation error: {exception.GetExceptionMessages()}, {exception}");
		}

		private static void AddInnerExceptions(Error error, Exception exception, int level = 0)
		{
			if (exception.InnerException == null) return;
			error.ExtraInfo[$"InnerException{level:00}"] = new[]
				{ $"{exception.InnerException.GetType().Name}: {exception.InnerException.Message}" };
			AddInnerExceptions(error, exception.InnerException, level++);
		}
	}
}
