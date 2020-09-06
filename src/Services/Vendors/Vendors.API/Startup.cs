using LoadLogic.Services.Vendors.API.Providers;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Services;

namespace LoadLogic.Services.Vendors.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            WebHostEnvironment = env;
            Configuration = configuration;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AuthSettings>(Configuration.GetSection("Auth"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
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
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        // .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
                });
            });

            services.AddControllers();
            services.AddMediatR(typeof(IHandler).Assembly);
            services.AddHttpContextAccessor();
            services.AddMemoryCache();


            services.AddTransient<IBlobService, StubbedBlobService>();
            services.AddTransient<IConnectionProvider, ConnectionProvider>();
            services.AddTransient(typeof(ICrudRepository<>), typeof(Repository<>));
            services.AddCustomProblemDetailsServices(WebHostEnvironment);

            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddLogging(builder => builder.AddSerilog());

            services.AddSingleton<ILogger>(sp =>
            {
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.FromLogContext();

                if (WebHostEnvironment.IsDevelopment())
                {
                    logger = logger.WriteTo.Console(theme: AnsiConsoleTheme.Code);
                }

                return logger.CreateLogger();
            });

            services.AddHealthChecks().AddDbContextCheck<VendorsContext>();
            services.AddDbContext<VendorsContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DatabaseConnection");

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(VendorsContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
                });
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors("default");

            app.UseAuthorization();
            app.UseProblemDetails();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiProvider.ApiVersionDescriptions.Reverse())
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }

                // required for serving swagger at the api root.
                options.RoutePrefix = string.Empty;

                options.OAuthAppName("Vendors API");
                options.OAuthScopeSeparator(" ");
                options.DisplayRequestDuration();
                options.DefaultModelRendering(ModelRendering.Model);
                options.DefaultModelExpandDepth(2);
            });
        }
    }
}
