using LoadLogic.Services.Vendors.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;

namespace LoadLogic.Services.Vendors.API
{
    /// <summary>
    /// Class containing the entry point for our server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configures and starts the web server.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Default logger used by host during startup
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Initializing host...");
            var host = CreateHostBuilder(args).Build();
            Log.Logger.Information("Successfully initialized host...");

            MigrateDatabase(host);
            host.Run();
        }

        /// <summary>
        /// Configures the web server.  (i.e., to use our Startup class)
        /// </summary>
        /// <param name="args"></param>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(b => b.AddJsonFile("appsettings.Local.json", true, false))
                .ConfigureWebHostDefaults(b => b.UseStartup<Startup>())
                .UseSerilog();

        public static void MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                var vendorsContext = scope.ServiceProvider.GetService<VendorsContext>();
                if (vendorsContext is null)
                {
                    logger.LogInformation($"Attempted DB migration unsuccessful. Unable to resolve {nameof(VendorsContext)}");
                }
                else
                {
                    vendorsContext.Database.Migrate();
                    logger.LogInformation("Attempted DB migration successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred migrating the DB.");
            }
        }
    }
}
