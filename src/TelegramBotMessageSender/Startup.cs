using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBotMessageSender.Initialization;
using TelegramBotMessageSender.Services.Config;
using TelegramBotMessageSender.Socks5Proxy.Config;

namespace TelegramBotMessageSender
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get;}

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"Config/appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddDataAnnotations()
                .AddJsonFormatters();

            services.Configure<TelegramConfig>(Configuration.GetSection("telegramConfig"));
            services.Configure<Socks5Config>(Configuration.GetSection("socks5Config"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            ContainerConfiguration.Configure(builder);
        }
    }
}
