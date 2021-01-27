using System;
using System.Linq;
using LoadLogic.Services.Ordering.API;
using LoadLogic.Services.Ordering.API.Providers;
using LoadLogic.Services.Ordering.Application.Interfaces;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using LoadLogic.Services.Ordering.Infrastructure.Persistence;
using LoadLogic.Services.Ordering.Infrastructure.Persistence.Repositories;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            services.AddOptions();
            services.ConfigureOptions<SwaggerConfiguration>();

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddMassTransit(ConfigureMassTransit);
            services.AddMassTransitHostedService();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddTransient<IConnectionProvider, ConnectionProvider>();
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

            services.AddCors(options =>
            {
                var allowedOrigins = Configuration
                    .GetValue<string>("Auth:CorsAllowedOrigins")
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .ToArray();

                options.AddPolicy("default", builder =>
                {
                    builder
                        .WithOrigins(allowedOrigins)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
            });

            services.AddHealthChecks()
                .AddDbContextCheck<OrderingContext>()
                .ForwardToPrometheus();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors("default");
            app.UseHttpMetrics();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapMetrics();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiProvider.ApiVersionDescriptions.Reverse())
                {
                    options.SwaggerEndpoint($"swagger/{description.GroupName}/swagger.json", description.GroupName);
                }

                // required for serving swagger at the api root.
                options.RoutePrefix = string.Empty;
                options.OAuthAppName("Ordering API");
                options.OAuthScopeSeparator(" ");
                options.DisplayRequestDuration();
                options.DefaultModelRendering(ModelRendering.Model);
                options.DefaultModelExpandDepth(2);
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
