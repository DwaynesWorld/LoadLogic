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
            busConfig.AddConsumer<OrderConfirmedIntegrationEventConsumer>();
            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.UsingRabbitMq((context, config) =>
            {
                config.ReceiveEndpoint("event-listener", e =>
                {
                    e.ConfigureConsumer<OrderConfirmedIntegrationEventConsumer>(context);
                });
            });
        }
    }
}
