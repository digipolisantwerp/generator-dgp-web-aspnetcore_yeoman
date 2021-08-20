using System;
using FOOBAR.Shared.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FOOBAR.Shared.Options
{
    public class AppSettings : SettingsBase
    {
        public const string PassThroughPrefix = "passthroughapi"; //this is not environment dependent; NO FORWARD SLASH
        public string AppName { get; set; }
        public string ApplicationId { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string DataDirectory { get; set; }
        public string TempDirectory { get; set; }
        public bool LogExceptions { get; set; }
        public bool DisableGlobalErrorHandling { get; set; }

        public static void RegisterConfiguration(IServiceCollection services, IConfigurationSection section, IWebHostEnvironment environment)
        {
            services.Configure<AppSettings>(settings =>
            {
                settings.LoadFromConfigSection(section);
                settings.OverrideFromEnvironmentVariables(environment);
            });
        }

        private void LoadFromConfigSection(IConfigurationSection section)
        {
            section.Bind(this);
        }

        private void OverrideFromEnvironmentVariables(IWebHostEnvironment environment)
        {
            AppName = GetValue(AppName, AppSettingsConfigKey.AppName, environment);
            ApplicationId = GetValue(ApplicationId, AppSettingsConfigKey.ApplicationId, environment);
            ApiUrl = GetValue(ApplicationId, AppSettingsConfigKey.ApiUrl, environment);
            ApiKey = GetValue(ApplicationId, AppSettingsConfigKey.ApiKey, environment);
            DataDirectory = GetValue(DataDirectory, AppSettingsConfigKey.DataDirectory, environment);
            TempDirectory = GetValue(TempDirectory, AppSettingsConfigKey.TempDirectory, environment);
            LogExceptions = GetValue(LogExceptions, AppSettingsConfigKey.LogExceptions, environment);
            DisableGlobalErrorHandling = GetValue(DisableGlobalErrorHandling, AppSettingsConfigKey.DisableGlobalErrorHandling, environment);
        }
    }
}
