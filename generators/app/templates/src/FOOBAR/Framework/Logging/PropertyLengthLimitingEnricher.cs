using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace FOOBAR.Framework.Logging
{
    public class PropertyLengthLimitingEnricher : ILogEventEnricher
    {
        private readonly int _maxStringLength;

        public PropertyLengthLimitingEnricher(int maximumStringLength)
        {
            if (maximumStringLength < 3) throw new ArgumentOutOfRangeException(nameof(maximumStringLength));
            _maxStringLength = maximumStringLength;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            List<LogEventProperty> updatedProperties = new List<LogEventProperty>();

            foreach (var property in logEvent.Properties)
            {
                if (property.Value is ScalarValue sv && sv.Value is string str && str.Length > _maxStringLength)
                {
                    updatedProperties.Add(new LogEventProperty(property.Key, new ScalarValue($"{str.Substring(0, _maxStringLength - 3)}...")));
                }
            }

            foreach (var update in updatedProperties)
            {
                logEvent.AddOrUpdateProperty(update);
            }
        }
    }
}
