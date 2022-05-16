using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FOOBAR.Shared.Options
{
	public class AppSettings
	{
		public const string PassThroughPrefix = "passthroughapi"; //this is not environment dependent; NO FORWARD SLASH
		public string AppName { get; set; }
		public string ApplicationId { get; set; }
		public string ApiUrl { get; set; }
		public string ApiKey { get; set; }
		public bool DisableGlobalErrorHandling { get; set; }
		public string TestingJwt { get; set; }

		public static void RegisterConfiguration(IServiceCollection services, IConfigurationSection section)
		{
			services.Configure<AppSettings>(settings =>
			{
				settings.LoadFromConfigSection(section);
				settings.OverrideFromEnvironmentVariables();
			});
		}

		private void LoadFromConfigSection(IConfiguration section)
		{
			section.Bind(this);
		}

		private void OverrideFromEnvironmentVariables()
		{
			var env = Environment.GetEnvironmentVariables();
			AppName = env.Contains("APPSETTINGS_APPNAME") ? env["APPSETTINGS_APPNAME"].ToString() : AppName;
			ApplicationId = env.Contains("APPSETTINGS_APPLICATIONID")
				? env["APPSETTINGS_APPLICATIONID"].ToString()
				: ApplicationId;
			ApiUrl = env.Contains("APPSETTINGS_API_URL") ? env["APPSETTINGS_API_URL"].ToString() : ApiUrl;
			ApiKey = env.Contains("APPSETTINGS_API_APIKEY") ? env["APPSETTINGS_API_APIKEY"].ToString() : ApiKey;
			DisableGlobalErrorHandling = env.Contains("APPSETTINGS_DISABLEGLOBALERRORHANDLING")
				? Convert.ToBoolean(env["APPSETTINGS_DISABLEGLOBALERRORHANDLING"].ToString())
				: DisableGlobalErrorHandling;
		}
	}
}
