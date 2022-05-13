using System;
using Microsoft.Extensions.Hosting;

namespace FOOBAR.Shared.Options
{
  public abstract class SettingsBase
  {
    public static string GetValue(string value, string configKey, IHostEnvironment env)
    {
      var configKeyValue = Environment.GetEnvironmentVariable(configKey);
      return configKeyValue ?? ((env.IsDevelopment())
        ? value
        : throw new ArgumentException($"Configuration error: invalid parameter '{configKey}'"));
    }

    public static int GetValue(int value, string configKey, IHostEnvironment env)
    {
      return int.TryParse(Environment.GetEnvironmentVariable(configKey), out var configKeyValue)
        ? configKeyValue
        : ((env.IsDevelopment())
          ? value
          : throw new ArgumentException($"Configuration error: invalid parameter '{configKey}'"));
    }

    public static bool GetValue(bool value, string configKey, IHostEnvironment env)
    {
      return bool.TryParse(Environment.GetEnvironmentVariable(configKey), out var configKeyValue)
        ? configKeyValue
        : ((env.IsDevelopment())
          ? value
          : throw new ArgumentException($"Configuration error: invalid parameter '{configKey}'"));
    }

    public static bool IsValidUri(string source, bool allowHttp = false) =>
      Uri.TryCreate(source, UriKind.Absolute, out var uriResult) &&
      (allowHttp || uriResult.Scheme == Uri.UriSchemeHttps);
  }
}
