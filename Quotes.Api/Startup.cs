using Funq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quotes.Api.ServiceInterface;
using ServiceStack;
using ServiceStack.Text;
using System;

namespace Quotes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });

            app.UseRouting();

            JsConfig.Init(new Config
            {
                TextCase = TextCase.PascalCase
            });
            JsConfig.AllowRuntimeType = type => true;
            JsConfig<Guid>.SerializeFn = guid => guid.ToString();
            JsConfig<Guid?>.SerializeFn = guid => guid?.ToString();
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("Quotes.Api", typeof(QuotesService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });

            Plugins.Add(new AutoQueryDataFeature());
        }
    }
}