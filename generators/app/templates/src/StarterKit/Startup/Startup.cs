using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StarterKit.AppStart;
using Digipolis.Utilities;
using Digipolis.WebApi;

namespace StarterKit
{
    public class Startup
    {
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
		    _applicationBasePath = appEnv.ApplicationBasePath;
		}
		
		private readonly string _applicationBasePath;       

        public void ConfigureServices(IServiceCollection services)
        {
            var configPath = Path.Combine(_applicationBasePath, "Configs");
			var config = new ConfigurationConfig(configPath);
			config.Configure(services);
			
            LoggingConfig.Configure(services);

            Factory.Configure(services);

			// camelCase JSON + RootObject
			services.AddMvc().Configure<MvcOptions>(options =>
			{
				var settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
				
				ListHelper.RemoveTypes(options.OutputFormatters, typeof(JsonOutputFormatter));
				
				var outputFormatter = new RootObjectJsonOutputFormatter() { SerializerSettings = settings };
				options.OutputFormatters.Insert(0, outputFormatter);
				
				ListHelper.RemoveTypes(options.InputFormatters, typeof(JsonInputFormatter));
				
				var inputFormatter = new RootObjectJsonInputFormatter() { SerializerSettings = settings };
				options.InputFormatters.Insert(0, inputFormatter);
			});
		}
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
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
