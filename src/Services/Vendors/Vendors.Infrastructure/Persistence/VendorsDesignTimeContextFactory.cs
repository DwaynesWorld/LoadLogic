using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LoadLogic.Services.Vendors.Infrastructure.Persistence
{
    /// <summary>
    /// Helper Class for database operations during development.
    /// Current directory should be Vendor service root.
    /// Command: dotnet ef migrations add MIGRATION_NAME -p Vendors.Infrastructure -s Vendors.API -o Persistence/Migrations
    /// </summary>
    public class VendorsDesignTimeContextFactory : IDesignTimeDbContextFactory<VendorsContext>
    {
        public VendorsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VendorsContext>();
            optionsBuilder.UseSqlServer(ConnectionString());
            return new VendorsContext(optionsBuilder.Options);
        }

        protected string ConnectionString()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var startupProject = Path.Combine(currentDirectory, @"../Vendors.API");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(startupProject)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.Local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetValue<string>("DatabaseConnection");
            return connectionString;
        }
    }
}
