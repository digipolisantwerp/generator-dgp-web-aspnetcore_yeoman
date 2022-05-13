using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace FOOBAR.Framework.Logging
{
  public class TypeEnricher : ILogEventEnricher
  {
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
      var types = new List<string>();

      switch (logEvent.Exception)
      {
        case null:
          types.Add("application");
          break;
        default:
          types.Add("technical");
          break;
      }

      logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
        "Type", types));
    }
  }
}
