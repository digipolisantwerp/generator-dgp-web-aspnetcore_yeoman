using System.IO;
using HelloWeb01.Options;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Toolbox.WebApi;

namespace StarterKit
{
    public class Startup
    {
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
            ApplicationBasePath = appEnv.ApplicationBasePath;
            ConfigPath = Path.Combine(ApplicationBasePath, "_config");
            
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
           services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
			services.AddMvc()
                .AddActionOverloading()
                .AddVersioning();
            
            services.AddBusinessServices();
            services.AddAutoMapper();
		}
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
            loggerFactory.AddSeriLog(Configuration.GetSection("SeriLog"));
            loggerFactory.AddConsole(Configuration.GetSection("ConsoleLogging"));
            loggerFactory.AddDebug(LogLevel.Debug);
            
			// CORS
            app.UseCors((policy) => {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });

            // static files en wwwroot
			app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider });
			app.UseStaticFiles(new StaticFileOptions { FileProvider = env.WebRootFileProvider });

            app.UseIISPlatformHandler();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
				routes.MapRoute(
					name: "api",
					template: "api/{controller}/{id?}");
			});
		}
        
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

