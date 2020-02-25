using System;
using System.Linq;
using Digipolis.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using FOOBAR.Logging;
using System.Collections.Generic;
using FOOBAR.Framework.Logging;

namespace FOOBAR
{
    public static class LoggingEngineExtensions
    {
        public static IServiceCollection AddLoggingEngine(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationLogger, ApplicationLogger>();

            services.AddSerilogExtensions(options =>
            {
                options.AddApplicationServicesEnricher();
                //options.AddAuthServiceEnricher();
                options.AddCorrelationEnricher();
                options.AddMessagEnricher(msgOptions => msgOptions.MessageVersion = "1");
            });

            return services;
        }

        public static ILoggerFactory AddLoggingEngine(this ILoggerFactory loggerFactory, IApplicationBuilder app, IApplicationLifetime appLifetime, IConfiguration config)
        {
            var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

            var systemLogSection = config.GetSection(Shared.Constants.Config.ConfigurationSection.SystemLog);
            var applicationLogSection = config.GetSection(Shared.Constants.Config.ConfigurationSection.ApplicationLog);

            var appLogger = typeof(ApplicationLogger).FullName;

            var loggingConfig = new LoggerConfiguration()
                            .Enrich.With(enrichers)
                            .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(systemLogSection).Filter.ByExcluding(Matching.FromSource(appLogger)))
                            .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(applicationLogSection).Filter.ByIncludingOnly(Matching.FromSource(appLogger)));


            foreach (var @override in systemLogSection.GetSection(Shared.Constants.Config.ConfigurationSection.MinimumLevel)?.GetSection(Shared.Constants.Config.ConfigurationSection.Override).GetChildren())
            {
                loggingConfig.MinimumLevel.Override(@override.Key, (LogEventLevel)Enum.Parse(typeof(LogEventLevel), @override.Value));
            }

            loggingConfig.Enrich.With(new PropertyLengthLimitingEnricher(4000));

            Log.Logger = loggingConfig.CreateLogger();

            loggerFactory.AddSerilog(dispose: true);

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            return loggerFactory;
        }

        /// <summary>
        /// overwrite logging configuration settings with the settings in the environment variables
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddLoggingConfiguration(this IConfigurationBuilder configurationBuilder, IHostingEnvironment hostingEnv)
        {
            var environmentDict = new Dictionary<string, string>();

            //first we read the json
            configurationBuilder.AddJsonFile("logging.json");

            //if this is deployed overwrite some settings from the environment variables
            if (!hostingEnv.IsDevelopment())
            {
                //CONSOLE
                FillFromEnvironment($"LOG_ELASTIC_CONSOLE_LEVEL_DEFAULT", "ConsoleLogging:LogLevel:Default", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_CONSOLE_LEVEL_SYSTEM", "ConsoleLogging:LogLevel:System", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_CONSOLE_LEVEL_MICROSOFT", "ConsoleLogging:LogLevel:Microsoft", environmentDict);

                // APPLICATION
                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_BUFFERPATH", "ApplicationLog:WriteTo:0:Args:bufferBaseFilename", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_HEADERS", "ApplicationLog:WriteTo:0:Args:connectionGlobalHeaders", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_URL", "ApplicationLog:WriteTo:0:Args:nodeUris", environmentDict);

                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_MINIMUMLEVEL_DEFAULT", "ApplicationLog:MinimumLevel:Default", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_MINIMUMLEVEL_OVERRIDE_SYSTEM", "ApplicationLog:MinimumLevel:Override:System", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_APPLICATION_MINIMUMLEVEL_OVERRIDE_MICROSOFT", "ApplicationLog:MinimumLevel:Override:Microsoft", environmentDict);

                // SYSTEM
                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_BUFFERPATH", "SystemLog:WriteTo:1:Args:bufferBaseFilename", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_HEADERS", "SystemLog:WriteTo:1:Args:connectionGlobalHeaders", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_URL", "SystemLog:WriteTo:1:Args:nodeUris", environmentDict);

                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_MINIMUMLEVEL_DEFAULT", "SystemLog:MinimumLevel:Default", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_MINIMUMLEVEL_OVERRIDE_SYSTEM", "SystemLog:MinimumLevel:Override:System", environmentDict);
                FillFromEnvironment($"LOG_ELASTIC_SYSTEM_MINIMUMLEVEL_OVERRIDE_MICROSOFT", "SystemLog:MinimumLevel:Override:Microsoft", environmentDict);

                configurationBuilder.AddInMemoryCollection(environmentDict);
            }

            return configurationBuilder;
        }

        private static void FillFromEnvironment(string envVarKey, string dictionaryKey, Dictionary<string, string> environmentDict)
        {
            var env = Environment.GetEnvironmentVariables();
            if (env.Contains(envVarKey))
            {
                environmentDict.Add(dictionaryKey, env[envVarKey].ToString());
            }
            else
            {
                throw new ArgumentNullException($"Configuration error: invalid parameter {envVarKey}");
            }
        }
    }
}
