
using LoadLogic.Services.Vendors.Application.Interfaces;
using Microsoft.Extensions.Options;
using SqlBulkToolsCore;
using System.Data;
using System.Data.SqlClient;

namespace LoadLogic.Services.Vendors.API.Providers
{
    /// <inheritdoc/>
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public ConnectionProvider(IOptions<ConnectionStrings> options)
        {
            _connectionString = options.Value.VendorsContext;
        }

        /// <inheritdoc/>
        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <inheritdoc/>
        public IBulkOperations GetBulkOperationConnection()
        {
            return new BulkOperations(_connectionString);
        }
    }
}
