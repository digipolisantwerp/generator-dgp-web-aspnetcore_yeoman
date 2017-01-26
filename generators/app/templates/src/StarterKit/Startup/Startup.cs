using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StarterKit.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Digipolis.Web;
using Digipolis.Web.Startup;
using Digipolis.ApplicationServices;
using Digipolis.Correlation;

namespace StarterKit
{
    public class Startup
    {
		public Startup(IHostingEnvironment env)
		{
            var appEnv = PlatformServices.Default.Application;
            ApplicationBasePath = appEnv.ApplicationBasePath;
            ConfigPath = Path.Combine(env.ContentRootPath, "_config");
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(ConfigPath)
                .AddJsonFile("logging.json")
                .AddJsonFile("app.json")
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
		}
		
        public IConfigurationRoot Configuration { get; private set; }
        public string ApplicationBasePath { get; private set; }
        public string ConfigPath { get; private set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(opt => Configuration.GetSection("AppSettings"));

            services.AddApplicationServices(opt => {
                opt.ApplicationId = "enter-your-application-id-here";
                opt.ApplicationName = "StarterKit";
            });

            services.AddCorrelation();

            services.AddLoggingEngine();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddApiExtensions(null, options =>
                {
                    options.DisableVersioning = true;
                    options.DisableGlobalErrorHandling = true;
                })
                .AddVersionEndpoint();;

            services.AddServices();
            services.AddAutoMapper();
            
            services.AddSwaggerGen();

            services.AddGlobalErrorHandling<ApiExceptionMapper>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
		{
            loggerFactory.AddConsole(Configuration.GetSection("ConsoleLogging"));
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddLoggingEngine(app, appLifetime, Configuration);
            
			// CORS
            app.UseCors((policy) => {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });

            app.UseApiExtensions();
            
            // static files en wwwroot
			app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider });
			app.UseStaticFiles(new StaticFileOptions { FileProvider = env.WebRootFileProvider });

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "api",
					template: "api/{controller}/{id?}");
			});

            app.UseSwagger();
            app.UseSwaggerUi();  
		}
    }
}

