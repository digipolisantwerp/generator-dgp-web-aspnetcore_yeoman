using System;
using Microsoft.Extensions.Logging;

namespace StarterKit.Logging
{
    public class ApplicationLogger : IApplicationLogger
    {
        public ApplicationLogger(ILogger<ApplicationLogger> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<ApplicationLogger> _logger;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }
    }
}
