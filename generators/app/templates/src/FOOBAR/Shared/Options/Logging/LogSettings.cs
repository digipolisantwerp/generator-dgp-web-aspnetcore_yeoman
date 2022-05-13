using System;
using System.Collections.Generic;
using FOOBAR.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FOOBAR.Shared.Options.Logging
{
	public class LogSettings : SettingsBase
	{
		public bool LogExceptions { get; set; }

		public RequestLogging RequestLogging { get; set; } = new RequestLogging();


		public static void RegisterConfiguration(IServiceCollection services, IConfigurationSection section,
			IHostEnvironment environment)
		{
			services.Configure<LogSettings>(settings =>
			{
				settings.LoadFromConfigSection(section);
				settings.OverrideFromEnvironmentVariables(environment);
			});
		}

		private void LoadFromConfigSection(IConfiguration section)
		{
			section.Bind(this);

			// load allowed headers lists manually from config
			RequestLogging.AllowedIncomingRequestHeaders = GetStringsFromConfigSection(section,
				"RequestLogging:AllowedIncomingRequestHeaders", RequestLogging.AllowedIncomingRequestHeaders);
			RequestLogging.AllowedIncomingResponseHeaders = GetStringsFromConfigSection(section,
				"RequestLogging:AllowedIncomingResponseHeaders", RequestLogging.AllowedIncomingResponseHeaders);
			RequestLogging.AllowedOutgoingRequestHeaders = GetStringsFromConfigSection(section,
				"RequestLogging:AllowedOutgoingRequestHeaders", RequestLogging.AllowedOutgoingRequestHeaders);
			RequestLogging.AllowedOutgoingResponseHeaders = GetStringsFromConfigSection(section,
				"RequestLogging:AllowedOutgoingResponseHeaders", RequestLogging.AllowedOutgoingResponseHeaders);
		}

		private static IEnumerable<string> GetStringsFromConfigSection(IConfiguration section, string subSectionKey,
			IEnumerable<string> defaultIfMissing)
		{
			var result = section.GetSection(subSectionKey).Get<IEnumerable<string>>();

			return result ?? defaultIfMissing;
		}

		private void OverrideFromEnvironmentVariables(IHostEnvironment environment)
		{
			// GENERAL
			LogExceptions = GetValue(LogExceptions, LogSettingsEnvVariableKey.LogExceptions, environment);

			// REQUEST LOGGING - GENERAL
			if (bool.TryParse(Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey.RequestLogging_LogPayload),
				    out var outgoingRequestLogPayload))
			{
				RequestLogging.LogPayload = outgoingRequestLogPayload;
			}

			if (bool.TryParse(
				    Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey.RequestLogging_LogPayloadOnError),
				    out var outgoingRequestLogPayloadOnError))
			{
				RequestLogging.LogPayloadOnError = outgoingRequestLogPayloadOnError;
			}

			// REQUEST LOGGING - INCOMING (middleware)
			if (bool.TryParse(
				    Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey.RequestLogging_Incoming_Enabled),
				    out var incomingRequestLoggingEnabled))
			{
				RequestLogging.IncomingEnabled = incomingRequestLoggingEnabled;
			}

			var allowedIncomingRequestHeaders =
				Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey
					.RequestLogging_Incoming_AllowedRequestHeaders);
			if (allowedIncomingRequestHeaders != null)
			{
				RequestLogging.AllowedIncomingRequestHeaders = !string.IsNullOrWhiteSpace(allowedIncomingRequestHeaders)
					? allowedIncomingRequestHeaders.Split(",")
					: null;
			}

			var allowedIncomingResponseHeaders =
				Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey
					.RequestLogging_Incoming_AllowedResponseHeaders);
			if (allowedIncomingResponseHeaders != null)
			{
				RequestLogging.AllowedIncomingResponseHeaders =
					!string.IsNullOrWhiteSpace(allowedIncomingResponseHeaders)
						? allowedIncomingResponseHeaders.Split(",")
						: null;
			}

			// REQUEST LOGGING - OUTGOING (delegating handler)
			if (bool.TryParse(
				    Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey.RequestLogging_OutgoingEnabled),
				    out var outgoingRequestLoggingEnabled))
			{
				RequestLogging.OutgoingEnabled = outgoingRequestLoggingEnabled;
			}

			var allowedOutgoingRequestHeaders =
				Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey
					.RequestLogging_Outgoing_AllowedRequestHeaders);
			if (allowedOutgoingRequestHeaders != null)
			{
				RequestLogging.AllowedOutgoingRequestHeaders = !string.IsNullOrWhiteSpace(allowedOutgoingRequestHeaders)
					? allowedOutgoingRequestHeaders.Split(",")
					: null;
			}

			var allowedOutgoingResponseHeaders =
				Environment.GetEnvironmentVariable(LogSettingsEnvVariableKey
					.RequestLogging_Outgoing_AllowedResponseHeaders);
			if (allowedOutgoingResponseHeaders != null)
			{
				RequestLogging.AllowedOutgoingResponseHeaders =
					!string.IsNullOrWhiteSpace(allowedOutgoingResponseHeaders)
						? allowedOutgoingResponseHeaders.Split(",")
						: null;
			}
		}
	}
}
