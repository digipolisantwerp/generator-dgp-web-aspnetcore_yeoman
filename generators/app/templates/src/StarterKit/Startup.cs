using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.StaticFiles;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// lambda middleware voor CORS
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
				context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "*" });
				context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "*" });
				await next();
			});

			// static files en wwwroot
			app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false, FileProvider = env.WebRootFileProvider });
			app.UseStaticFiles(new StaticFileOptions { FileProvider = env.WebRootFileProvider });

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action}/{id?}",
					defaults: new { controller = "Home", action = "Index" });

				routes.MapRoute(
					name: "api",
					template: "{controller}/{id?}");
			});

			
		}

        public void ConfigureServices(IServiceCollection services)
        {
            var configPath = Path.Combine(_applicationBasePath, "Configs");
			var config = new ConfigurationConfig(configPath);
			config.Configure(services);
			
            LoggingConfig.Configure(services);
            AutoMapperConfiguration.Configure();

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
    }
}
