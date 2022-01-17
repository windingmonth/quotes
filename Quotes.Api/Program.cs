using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using ServiceStack;

namespace Quotes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseModularStartup<Startup, StartupActivator>();
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment;
                builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            }).UseNLog();
    }

    public class StartupActivator : ModularStartupActivator
    {
        public StartupActivator(IConfiguration configuration) : base(configuration)
        {
        }
    }
}