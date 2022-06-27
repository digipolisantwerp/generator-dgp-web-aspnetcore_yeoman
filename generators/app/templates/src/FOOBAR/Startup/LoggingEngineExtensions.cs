using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Serilog;
using FOOBAR.Framework.Logging;
using FOOBAR.Shared.Constants;
using FOOBAR.Shared.Options;
using FOOBAR.Shared.Options.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace FOOBAR.Startup
{
	public static class LoggingExtensions
	{
		/// <summary>
		/// configure logging services in Startup
		/// </summary>
		public static void AddLogging(this IServiceCollection services,
			IConfiguration config,
			IHostEnvironment environment)
		{
			// initialize Logsettings options object and overwrite from environment variables
			LogSettings.RegisterConfiguration(services,
				config.GetSection(Shared.Constants.Config.ConfigurationSection.LogSettings), environment);

			// add serilog enrichers
			services.AddSerilogExtensions(options =>
			{
				options.AddApplicationServicesEnricher();
				//options.AddAuthServiceEnricher();
				options.AddCorrelationEnricher();
				options.AddMessagEnricher(msgOptions => msgOptions.MessageVersion = "1");
			});
		}

		/// <summary>
		/// configure logging in Startup: initialize serilog
		/// </summary>
		public static void UseLogging(this ILoggerFactory loggerFactory,
			IApplicationBuilder app,
			IHostApplicationLifetime appLifetime,
			IConfiguration config,
			IHostEnvironment hostingEnv)
		{
			// SERILOG - initialize
			var enrich = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

			var logSection = hostingEnv.EnvironmentName == Environments.Development
				? Shared.Constants.Config.ConfigurationSection.SerilogDev
				: Shared.Constants.Config.ConfigurationSection.Serilog;

			Log.Logger = new LoggerConfiguration()
				.Enrich.With(enrich)
				.Enrich.With(new TypeEnricher())
				.ReadFrom.Configuration(config, logSection)
				.CreateLogger();

			loggerFactory.AddSerilog(dispose: true);

			appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
		}

		/// <summary>
		/// load logging configuration from json and overwrite serilog log levels from environment variables
		/// </summary>
		/// <param name="configurationBuilder"></param>
		/// <param name="hostingEnv"></param>
		/// <returns></returns>
		public static void AddLoggingConfiguration(this IConfigurationBuilder configurationBuilder,
			IHostEnvironment hostingEnv)
		{
			var env = Environment.GetEnvironmentVariables();

			var environmentDict = new Dictionary<string, string>();

			// overwrite serilog log levels from the environment variables
			if (hostingEnv.EnvironmentName != Environments.Development)
			{
				ConfigUtil.FillFromEnvironment(LogSettingsEnvVariableKey.SeriLog_MinimumLevel_Default,
					"Serilog:MinimumLevel:Default", environmentDict);
				ConfigUtil.FillFromEnvironment(LogSettingsEnvVariableKey.SeriLog_MinimumLevel_Override_System,
					"Serilog:MinimumLevel:Override:System", environmentDict);
				ConfigUtil.FillFromEnvironment(LogSettingsEnvVariableKey.SeriLog_MinimumLevel_Override_Microsoft,
					"Serilog:MinimumLevel:Override:Microsoft", environmentDict);
			}

			// load in this order so that json-settings will be overridden with environment settings when getting the configuration section
			configurationBuilder.AddJsonFile("logging.json");
			configurationBuilder.AddInMemoryCollection(environmentDict);
		}
	}
}
