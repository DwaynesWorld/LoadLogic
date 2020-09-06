using System.Data;
using LoadLogic.Services.Vendors.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SqlBulkToolsCore;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class TestDbConnectionProvider : IConnectionProvider
    {
        private readonly DbContext _context;

        public TestDbConnectionProvider(DbContext context)
        {
            _context = context;
        }

        public IBulkOperations GetBulkOperationConnection()
        {
            throw new System.NotImplementedException();
        }

        public IDbConnection GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }
    }
}
