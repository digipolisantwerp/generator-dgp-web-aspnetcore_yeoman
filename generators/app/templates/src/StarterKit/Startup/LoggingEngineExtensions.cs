using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Filters;
using StarterKit.Logging;

namespace StarterKit
{
  public static class LoggingEngineExtensions
  {
    public static IServiceCollection AddLoggingEngine(this IServiceCollection services)
    {
      services.AddSingleton<IApplicationLogger, ApplicationLogger>();

      services.AddSerilogExtensions(options =>
      {
        options.AddApplicationServicesEnricher();
        options.AddAuthServiceEnricher();
        options.AddCorrelationEnricher();
        options.AddMessagEnricher(msgOptions => msgOptions.MessageVersion = "1");
      });

      return services;
    }

    public static ILoggerFactory AddLoggingEngine(this ILoggerFactory loggerFactory, IApplicationBuilder app, IApplicationLifetime appLifetime, IConfiguration config)
    {
      var enrichers = app.ApplicationServices.GetServices<ILogEventEnricher>().ToArray();

      var systemLogSection = config.GetSection("SystemLog");
      var applicationLogSection = config.GetSection("ApplicationLog");

      var appLogger = typeof(ApplicationLogger).FullName;

      Log.Logger = new LoggerConfiguration()
                      .Enrich.With(enrichers)
                      .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(systemLogSection).Filter.ByExcluding(Matching.FromSource(appLogger)))
                      .WriteTo.Logger(l => l.ReadFrom.ConfigurationSection(applicationLogSection).Filter.ByIncludingOnly(Matching.FromSource(appLogger)))
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
    public static IConfigurationBuilder AddLoggingConfiguration(this IConfigurationBuilder configurationBuilder)
    {
      var env = Environment.GetEnvironmentVariables();

      var environmentDict = new Dictionary<string, string>();

      // APPLICATION
      if (env.Contains($"LOG_ELASTIC_APPLICATION_BUFFERPATH"))
      {
        environmentDict.Add("ApplicationLog:WriteTo:0:Args:bufferBaseFilename", env[$"LOG_ELASTIC_APPLICATION_BUFFERPATH"].ToString());
      }
      if (env.Contains($"LOG_ELASTIC_APPLICATION_HEADERS"))
      {
        environmentDict.Add("ApplicationLog:WriteTo:0:Args:connectionHeaders", env[$"LOG_ELASTIC_APPLICATION_HEADERS"].ToString());
      }
      if (env.Contains($"LOG_ELASTIC_APPLICATION_URL"))
      {
        environmentDict.Add("ApplicationLog:WriteTo:0:Args:nodeUris", env[$"LOG_ELASTIC_APPLICATION_URL"].ToString());
      }

      // SYSTEM
      if (env.Contains($"LOG_ELASTIC_SYSTEM_BUFFERPATH"))
      {
        environmentDict.Add("SystemLog:WriteTo:1:Args:bufferBaseFilename", env[$"LOG_ELASTIC_SYSTEM_BUFFERPATH"].ToString());
      }
      if (env.Contains($"LOG_ELASTIC_SYSTEM_HEADERS"))
      {
        environmentDict.Add("SystemLog:WriteTo:1:Args:connectionHeaders", env[$"LOG_ELASTIC_SYSTEM_HEADERS"].ToString());
      }
      if (env.Contains($"LOG_ELASTIC_SYSTEM_URL"))
      {
        environmentDict.Add("SystemLog:WriteTo:1:Args:nodeUris", env[$"LOG_ELASTIC_SYSTEM_URL"].ToString());
      }

      // load in this order so that json-settings will be overridden with environment settings when getting the configuration section
      configurationBuilder.AddJsonFile("logging.json");
      configurationBuilder.AddInMemoryCollection(environmentDict);
      return configurationBuilder;
    }
  }
}
