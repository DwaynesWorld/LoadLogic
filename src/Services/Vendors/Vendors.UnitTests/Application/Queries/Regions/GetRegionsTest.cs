using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.Regions;
using LoadLogic.Services.Vendors.Application.Queries.Regions;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    [Collection("DapperSqlite")]
    public class GetRegionsTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetRegionsTest()
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
        public async Task GetRegions_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetRegions();
            var handler = new GetRegionsHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRegions_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<Region>(context);
            var command = new CreateRegion("SE", "Southeast");
            var commandHandler = new CreateRegionHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetRegions();
            var queryHandler = new GetRegionsHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("SE", result.First().Code);
            Assert.Equal("Southeast", result.First().Description);
        }

        [Fact]
        public async Task GetRegions_ShouldReturnMultipleExistingItems()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<Region>(context);
            var command = new CreateRegion("SE", "Southeast");
            var commandHandler = new CreateRegionHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            var command2 = new CreateRegion("NE", "Northeast");
            var commandHandler2 = new CreateRegionHandler(mediator, repo);
            await commandHandler2.Handle(command2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetRegions();
            var queryHandler = new GetRegionsHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal(2, (int)result.Count());

            var x = result.Where(x => x.Code == "SE").FirstOrDefault();
            Assert.NotNull(x);
            Assert.Equal("SE", x!.Code);
            Assert.Equal("Southeast", x.Description);

            var x2 = result.Where(x => x.Code == "NE").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("NE", x2!.Code);
            Assert.Equal("Northeast", x2.Description);
        }
    }
}
