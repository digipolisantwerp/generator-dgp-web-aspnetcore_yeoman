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
using Serilog.Filters;
using StarterKit.Logging;

namespace StarterKit
{
    public static class LoggingEngineExtensions
    {
        public static IServiceCollection AddLoggingEngine(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationLogger, ApplicationLogger>();

            services.AddSerilogExtensions(options => {
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
    }
}
