using Digipolis.ApplicationServices;
using Digipolis.Auth;
using Digipolis.Auth.Extensions;
using Digipolis.Auth.Options;
using Digipolis.Correlation;
using FOOBAR.Framework.Logging.Middleware;
using FOOBAR.Shared.Extensions;
using FOOBAR.Shared.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FOOBAR.Startup
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; private set; }
		public string ApplicationBasePath { get; private set; }
		public string ConfigPath { get; private set; }
		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			//https://github.com/dotnet/aspnetcore/issues/7644
			//https://github.com/dotnet/aspnetcore/issues/8302
			// If using Kestrel:
			services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

			AppSettings.RegisterConfiguration(services,
				Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.AppSettings));

			var appSettings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>().Value;

			services.AddApplicationServices(opt =>
			{
				opt.ApplicationId = appSettings.ApplicationId;
				opt.ApplicationName = appSettings.AppName;
			});

			services.AddCorrelation();
			services.AddCors();

			services.AddLogging(Configuration, Environment);

			services.AddAuthFromOptions(options =>
			{
				var configSection = Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.Auth);
				configSection.Bind(options);
				AuthSettingsConfig.SetConfig(options, appSettings);
			}, devOptions =>
			{
				var configSection =
					Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.DevPermissions);
				configSection.Bind(devOptions);
			});

			services.AddAuthorization(services.BuildOAuthPolicies());

			services.AddMvc(options => options.EnableEndpointRouting = false)
				.AddNewtonsoftJson(options =>
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddServices();
			services.AddHelperServices();
			services.AddProgressiveWebApp();

			services.ConfigureNonBreakingSameSiteCookies();

			//Global error handling in the BFF adds chunked encoding errors with the passthrough
			//please keep that in mind if you are going to define a BFF API.
			//services.AddGlobalErrorHandling<ApiExceptionMapper>();
		}

		//This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
			IOptions<AppSettings> settings, IOptions<AuthOptions> authOptions, IHostApplicationLifetime appLifetime,
			ILogger<Startup> logger)
		{
			loggerFactory.UseLogging(app, appLifetime, Configuration, Environment);

			//used for docker
			app.UseForwardedHeaders();
			Serilog.Debugging.SelfLog.Enable(System.Console.Out);

			app.UseMiddleware<IncomingRequestLogger>();

			var appName = settings.Value.AppName;

			//application lifetime events
			appLifetime.ApplicationStarted.Register(() => logger.LogInformation($"Application {appName} Started"));
			appLifetime.ApplicationStopped.Register(() => logger.LogInformation($"Application {appName} Stopped"));
			appLifetime.ApplicationStopping.Register(() => logger.LogInformation($"Application {appName} Stopping"));

			app.UseCorrelation();
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = SameSiteMode.Lax
			});

			app.UseApiExtensions();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			// static files en wwwroot
			app.UseFileServer(new FileServerOptions
				{ EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider });
			app.UseStaticFiles(new StaticFileOptions { FileProvider = env.WebRootFileProvider });

			app.UseRouting();

			// CORS
			app.UseCors(policy =>
			{
				policy.AllowAnyHeader();
				policy.AllowAnyMethod();
				policy.SetIsOriginAllowed(origin => true);
				policy.AllowCredentials();
			});

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseOAuth(authOptions);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"BackendApi",
					AppSettings.PassThroughPrefix + "/{*queryvalues}",
					new { controller = "PassThrough", action = "Handle" });
				routes.MapRoute(
					"default",
					"{*catchall}",
					new { controller = "Home", action = "Index" }
				);
			});
		}
	}
}
