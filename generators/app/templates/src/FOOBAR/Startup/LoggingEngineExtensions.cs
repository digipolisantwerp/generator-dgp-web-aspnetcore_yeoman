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
using FOOBAR.Enrichers;
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
          var enrich = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

          Log.Logger = new LoggerConfiguration()
            .Enrich.With(enrich)
            .Enrich.With(new TypeEnricher())
            .ReadFrom.Configuration(config)
            .CreateLogger();

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
                FillFromEnvironment($"LOG_SYSTEM_BUFFERPATH", "Serilog:WriteTo:1:Args:bufferBaseFilename", environmentDict);
                FillFromEnvironment($"LOG_SYSTEM_HEADERS", "Serilog:WriteTo:1:Args:connectionGlobalHeaders", environmentDict);
                FillFromEnvironment($"LOG_SYSTEM_URL", "Serilog:WriteTo:1:Args:nodeUris", environmentDict);

                FillFromEnvironment($"LOG_SYSTEM_MINIMUMLEVEL_DEFAULT", "Serilog:MinimumLevel:Default", environmentDict);
                FillFromEnvironment($"LOG_SYSTEM_MINIMUMLEVEL_OVERRIDE_SYSTEM", "Serilog:MinimumLevel:Override:System", environmentDict);
                FillFromEnvironment($"LOG_SYSTEM_MINIMUMLEVEL_OVERRIDE_MICROSOFT", "Serilog:MinimumLevel:Override:Microsoft", environmentDict);

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
