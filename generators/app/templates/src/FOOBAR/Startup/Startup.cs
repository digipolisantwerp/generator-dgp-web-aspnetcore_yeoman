using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FOOBAR.Shared.Options;
using Digipolis.Web;
using Digipolis.ApplicationServices;
using Digipolis.Correlation;
using Digipolis.Authentication.OAuth;
using Digipolis.Authentication.OAuth.Options;
using FOOBAR.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace FOOBAR
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
			      Environment = env;
        }

        public IConfiguration Configuration { get; private set; }
        public string ApplicationBasePath { get; private set; }
        public string ConfigPath { get; private set; }
		    public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            AppSettings.RegisterConfiguration(services, Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.AppSettings));

            var appSettings = services.BuildServiceProvider().GetService<IOptions<AppSettings>>().Value;

            services.AddApplicationServices(opt =>
            {
                opt.ApplicationId = appSettings.ApplicationId;
                opt.ApplicationName = appSettings.AppName;
            });

            services.AddCorrelation();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.ConsoleLogging));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddLoggingEngine();

             services.AddOAuth(options => {
                var configSection = Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.Auth);
                configSection.Bind(options);
                AuthSettingsConfig.SetConfig(options);
                options.ApiKey = appSettings.ApiKey;
            }, devoptions => {
                var configSection = Configuration.GetSection(Shared.Constants.Config.ConfigurationSection.DevPermissions);
                configSection.Bind(devoptions);
            });

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddApiExtensions(null, options =>
                {
                    options.DisableVersioning = true;
                });

            services.AddServices();
            services.AddHelperServices();
            services.AddProgressiveWebApp();

            //Global error handling in the BFF adds chunked encoding errors with the passthrough
            //please keep that in mind if you are going to define a BFF API.
            //services.AddGlobalErrorHandling<ApiExceptionMapper>();

            services.ConfigureNonBreakingSameSiteCookies();
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AppSettings> settings, IOptions<OAuthOptions> oauthOptions, IApplicationLifetime appLifetime, ILogger<Startup> logger)
        {
			      //used for docker
            app.UseForwardedHeaders();

            loggerFactory.AddLoggingEngine(app, appLifetime, Configuration);
            Serilog.Debugging.SelfLog.Enable(System.Console.Out);

            var appName = app.ApplicationServices.GetService<IOptions<AppSettings>>().Value.AppName;

            //application lifetime events
            appLifetime.ApplicationStarted.Register(() => logger.LogInformation($"Application {appName} Started"));
            appLifetime.ApplicationStopped.Register(() => logger.LogInformation($"Application {appName} Stopped"));
            appLifetime.ApplicationStopping.Register(() => logger.LogInformation($"Application {appName} Stopping"));

            app.UseCookiePolicy();

            // CORS
            app.UseCors((policy) =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });

            app.UseOAuth(oauthOptions);

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
            app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = env.WebRootFileProvider });

            app.UseMvc(routes =>
            {
                routes.MapRoute("BackendApi", AppSettings.PassThroughPrefix + "/{*queryvalues}", new { controller = "PassThrough", action = "Handle" });
                routes.MapRoute(
                    "default",
                    "{*catchall}",
                    new { controller = "Home", action = "Index" }
                );
            });
        }
    }
}
