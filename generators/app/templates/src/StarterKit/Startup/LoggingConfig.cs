using System;
using StarterKit.Utilities.Configs;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System.IO;

namespace StarterKit.AppStart
{
    public class LoggingConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(CreateLogger);
        }

        private static ILogger CreateLogger(IServiceProvider serviceProvider)
        {
            var logConfig = serviceProvider.GetService<ILoggingConfiguration>();
            if ( logConfig == null ) throw new Exception("logConfig kan niet geïnstantieerd worden via de ServiceProvider.");

            var target = new FileTarget();
            target.FileName = Path.Combine(logConfig.FileTarget.Path, logConfig.FileTarget.FileName);
            target.Layout = new SimpleLayout(logConfig.FileTarget.Layout);
            target.CreateDirs = true;

            // ToDo : rolling archivering nog toevoegen

            var rule = new LoggingRule("*", GetLogLevel(logConfig.FileTarget.Level), target);


            var nLogConfig = new NLog.Config.LoggingConfiguration();
            nLogConfig.AddTarget(logConfig.FileTarget.Name, target);
            nLogConfig.LoggingRules.Add(rule);

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog(new NLog.LogFactory(nLogConfig));

            var logger = loggerFactory.CreateLogger(logConfig.Name);
            return logger;
        }

        private static NLog.LogLevel GetLogLevel(string loglevel)
        {
            if ( loglevel == "debug" ) return NLog.LogLevel.Debug;
            if ( loglevel == "error" ) return NLog.LogLevel.Error;
            if ( loglevel == "fatal" ) return NLog.LogLevel.Fatal;
            if ( loglevel == "info" ) return NLog.LogLevel.Info;
            if ( loglevel == "off" ) return NLog.LogLevel.Off;
            if ( loglevel == "trace" ) return NLog.LogLevel.Trace;
            if ( loglevel == "warn" ) return NLog.LogLevel.Warn;
            return NLog.LogLevel.Info;
        }
    }
}