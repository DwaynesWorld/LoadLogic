using System;
using Dapper;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class DapperSqliteMapperFixture
    {
        /// <summary>
        /// Initialize Dapper SqlMapper
        /// </summary>
        public DapperSqliteMapperFixture()
        {

        }
    }

    /// <summary>
    /// This class has no code, and is never created. Its purpose is simply
    /// to be the place to apply [CollectionDefinition] and all the
    /// ICollectionFixture<> interfaces.
    /// </summary>
    [CollectionDefinition("DapperSqlite")]
    public class DapperSqliteCollection : ICollectionFixture<DapperSqliteMapperFixture> { }
}
