using LoadLogic.Services.Ordering.Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoadLogic.Services.Ordering.API.Providers
{
    /// <inheritdoc/>
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public ConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DATABASE_CONNECTION_STRING");
        }

        /// <inheritdoc/>
        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // /// <inheritdoc/>
        // public IBulkOperations GetBulkOperationConnection()
        // {
        //     return new BulkOperations(_connectionString);
        // }
    }
}
