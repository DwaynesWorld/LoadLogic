using System;
using LoadLogic.Services.Ordering.Application.Abstractions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using LoadLogic.Services.Ordering.Infrastructure.Persistence;
using LoadLogic.Services.Ordering.Infrastructure.Persistence.Repositories;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;

namespace LoadLogic.Services.Ordering
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
            services.AddMediatR(typeof(Startup));
            services.AddMassTransit(ConfigureMassTransit);
            services.AddMassTransitHostedService();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddHealthChecks()
                .AddDbContextCheck<OrderingContext>()
                .ForwardToPrometheus();

            services.AddDbContext<OrderingContext>(options =>
            {
                var connectionString = Configuration.GetValue<string>("DATABASE_CONNECTION_STRING");
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.UseNetTopologySuite();
                    sqlOptions.MigrationsAssembly(typeof(OrderingContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapMetrics();
            });
        }

        protected void ConfigureMassTransit(IServiceCollectionBusConfigurator busConfig)
        {
            busConfig.SetKebabCaseEndpointNameFormatter();
            busConfig.UsingRabbitMq((context, factoryConfig) =>
            {
                var connection = Configuration.GetValue<string>("EVENTBUS_CONNECTION_STRING");
                var username = Configuration.GetValue<string>("EVENTBUS_USERNAME");
                var password = Configuration.GetValue<string>("EVENTBUS_PASSWORD");

                factoryConfig.UseHealthCheck(context);
                factoryConfig.Host(connection, hostConfig =>
                {
                    hostConfig.Username(username);
                    hostConfig.Password(password);
                });
            });
        }
    }
}
