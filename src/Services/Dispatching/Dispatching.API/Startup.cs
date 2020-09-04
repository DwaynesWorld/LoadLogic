using LoadLogic.Services.Dispatching.Application.Abstractions;
using LoadLogic.Services.Dispatching.Application.IntegrationEventConsumers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoadLogic.Services.Dispatching.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMediatR(typeof(IHandler).Assembly);
            services.AddMassTransit(ConfigureMassTransit);
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected void ConfigureMassTransit(IServiceCollectionBusConfigurator busConfig)
        {
            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.UsingRabbitMq((context, factoryConfig) =>
            {
                var connection = Configuration.GetValue<string>("EventBusConnection");
                var username = Configuration.GetValue<string>("EventBusUserName");
                var password = Configuration.GetValue<string>("EventBusPassword");

                factoryConfig.Host(connection, hostConfig =>
                {
                    hostConfig.Username(username);
                    hostConfig.Password(password);
                });

                factoryConfig.ReceiveEndpoint("order-event-listener", e =>
                {
                    e.Consumer<OrderConfirmedIntegrationEventConsumer>();
                });
            });
        }
    }
}
