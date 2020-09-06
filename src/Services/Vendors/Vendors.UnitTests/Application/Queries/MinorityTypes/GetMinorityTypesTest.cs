using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Queries.MinorityTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    [Collection("DapperSqlite")]
    public class GetMinorityTypesTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetMinorityTypesTest()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<VendorsContext>().UseSqlite(_connection).Options;
            using var context = new VendorsContext(_options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        [Fact]
        public async Task GetMinorityTypes_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetMinorityTypes();
            var handler = new GetMinorityTypesHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMinorityTypes_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<MinorityType>(context);
            var command = new CreateMinorityType("DBE", "Disadvantaged Business Enterprise");
            var commandHandler = new CreateMinorityTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetMinorityTypes();
            var queryHandler = new GetMinorityTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("DBE", result.First().Code);
            Assert.Equal("Disadvantaged Business Enterprise", result.First().Description);
        }

        [Fact]
        public async Task GetMinorityTypes_ShouldReturnMultipleExistingItems()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<MinorityType>(context);
            var command = new CreateMinorityType("DBE", "Disadvantaged Business Enterprise");
            var commandHandler = new CreateMinorityTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            var command2 = new CreateMinorityType("WBE", "Women's Business Enterprise");
            var commandHandler2 = new CreateMinorityTypeHandler(mediator, repo);
            await commandHandler2.Handle(command2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetMinorityTypes();
            var queryHandler = new GetMinorityTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal(2, (int)result.Count());

            var x = result.Where(x => x.Code == "DBE").FirstOrDefault();
            Assert.NotNull(x);
            Assert.Equal("DBE", x!.Code);
            Assert.Equal("Disadvantaged Business Enterprise", x.Description);

            var x2 = result.Where(x => x.Code == "WBE").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("WBE", x2!.Code);
            Assert.Equal("Women's Business Enterprise", x2.Description);

        }
    }
}
