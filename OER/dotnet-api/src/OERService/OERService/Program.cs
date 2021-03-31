using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;

namespace OERService
{
	public class Program
    {
		Program()
		{
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .CreateLogger();
            Log.Information("OER Service is running");

        }
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();    
      
    }
}
