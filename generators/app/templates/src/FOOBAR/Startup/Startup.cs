using System;
using System.IO;
using Digipolis.ApplicationServices;
using Digipolis.Auth;
using Digipolis.Auth.Extensions;
using Digipolis.Auth.Options;
using Digipolis.Correlation;
using FOOBAR.Shared.Constants;
using FOOBAR.Shared.Extensions;
using FOOBAR.Shared.Options;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog.Debugging;

namespace FOOBAR.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
            ConfigPath = Path.Combine(env.ContentRootPath, "_config");
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        private string ConfigPath { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Read settings

            AppSettings.RegisterConfiguration(services, Configuration.GetSection(ConfigurationSectionKey.AppSettings),
                Environment);
            services.AddSingleton(Configuration);

            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            #endregion

            #region Add Correlation and application services

            services.AddCorrelation(options =>
            {
                options.CorrelationHeaderRequired = !Environment.IsDevelopment();
            });

            services.AddApplicationServices(opt =>
            {
                opt.ApplicationId = appSettings.ApplicationId;
                opt.ApplicationName = appSettings.AppName;
            });

            #endregion

            #region Logging

            services.AddLoggingEngine();

            #endregion

            #region Add routing and versioning



            #endregion

            #region Authorization & Authentication

            services.AddAuthFromJsonFile(options =>
            {
                options.BasePath = ConfigPath;
                options.FileName = "auth.json";
                options.Section = ConfigurationSectionKey.Auth;

                //  var configSection = Configuration.GetSection(ConfigConstants.Auth);
                // configSection.Bind(options);
                // AuthSettingsConfig.SetConfig(options);
            });
            services.Configure<AuthOptions>(options => AuthSettingsConfig.SetConfig(options));
            services.AddAuthorization(services.BuildOAuthPolicies());

            #endregion

            // services
            //     .AddRouting(options =>
            //     {
            //         options.LowercaseUrls = true;
            //         options.LowercaseQueryStrings = true;
            //     })
            //     .AddControllers(options =>
            //     {
            //         options.EnableEndpointRouting = false;
            //     })
            //     .AddNewtonsoftJson(options =>
            //     {
            //         options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //         options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //         options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //         options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
            //         options.SerializerSettings.Converters.Add(new StringEnumConverter());
            //     });
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.UseApiBehavior = false;
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            // .AddJsonOptions(options =>
            // {
            //     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // })
            // .AddApiExtensions(null, options =>
            // {
            //     options.DisableVersioning = true;
            // });

            services.AddServices();
            services.AddHelperServices();
            services.AddProgressiveWebApp();

            //Global error handling in the BFF adds chunked encoding errors with the passthrough
            //please keep that in mind if you are going to define a BFF API.
            //services.AddGlobalErrorHandling<ApiExceptionMapper>();

            services.ConfigureNonBreakingSameSiteCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.IsEssential = true;
                // we need to disable to allow iframe for authorize requests
                options.Cookie.SameSite = (SameSiteMode) (-1);
            });
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
            IOptions<AppSettings> settings, IOptions<AuthOptions> authOptions, IHostApplicationLifetime appLifetime,
            ILogger<Startup> logger)
        {
            //used for docker
            app.UseForwardedHeaders();

            loggerFactory.AddLoggingEngine(app, appLifetime, Configuration);
            SelfLog.Enable(Console.Out);

            var appName = settings.Value.AppName;

            //application lifetime events
            appLifetime.ApplicationStarted.Register(() => logger.LogInformation($"Application {appName} Started"));
            appLifetime.ApplicationStopped.Register(() => logger.LogInformation($"Application {appName} Stopped"));
            appLifetime.ApplicationStopping.Register(() => logger.LogInformation($"Application {appName} Stopping"));

            // CORS
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });

            // app.UseRouting();

            app.UseAuthentication();
            app.UseOAuth(authOptions);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                RequireHeaderSymmetry = true,
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            // static files en wwwroot
            app.UseFileServer(new FileServerOptions
                {EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider});
            app.UseStaticFiles(new StaticFileOptions {FileProvider = env.WebRootFileProvider});

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute("BackendApi", AppSettings.PassThroughPrefix + "/{*queryvalues}",
            //         new {controller = "PassThrough", action = "Handle"});
            //     endpoints.MapControllerRoute(
            //         "default",
            //         "{*catchall}",
            //         new {controller = "Home", action = "Index"}
            //     );
            // });
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
