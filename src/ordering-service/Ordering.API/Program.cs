using LoadLogic.Services.Ordering.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using System.IO;
using System;

namespace LoadLogic.Services.Ordering.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace!;
        public static readonly string AppName = Namespace;

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = CreateHostBuilder(configuration, args);

                Log.Information("Attempting DB Migrations...");
                MigrateDatabase(host);

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHost CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(b => b.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
                .UseSerilog()
                .Build();

        private static IConfiguration GetConfiguration()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration.GetValue<string>("Serilog:SeqServerUrl");

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static void MigrateDatabase(IHost host)
        {
            try
            {
                using var scope = host.Services.CreateScope();
                var orderingContext = scope.ServiceProvider.GetService<OrderingContext>();
                if (orderingContext is null)
                {
                    Log.Information($"Attempted DB migration unsuccessful. Unable to resolve {nameof(OrderingContext)}");
                }
                else
                {
                    orderingContext.Database.Migrate();
                    Log.Information("Attempted DB migration successfully.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the DB.");
            }
        }
    }
}
