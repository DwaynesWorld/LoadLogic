using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Ordering.Infrastructure.Persistence
{
    /// <summary>
    /// Helper Class for database operations during development.
    /// Current directory should be Ordering root.
    /// Command: dotnet ef migrations add MIGRATION_NAME -p Ordering.Infrastructure -s Ordering.Api -o Persistence/Migrations
    /// </summary>
    public class OrderingDesignTimeContextFactory : IDesignTimeDbContextFactory<OrderingContext>
    {
        public OrderingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderingContext>();
            optionsBuilder.UseSqlServer(ConnectionString());
            return new OrderingContext(optionsBuilder.Options, new StubbedMediator());
        }

        protected string ConnectionString()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var startupProject = Path.Combine(currentDirectory, @"../Ordering.API");

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

        private class StubbedMediator : IMediator
        {
            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task Publish<T>(T notification, CancellationToken cancellationToken = default)
                where T : INotification
            {
                return Task.CompletedTask;
            }

            public Task<T> Send<T>(IRequest<T> request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(T)!);
            }

            public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }
        }
    }
}
