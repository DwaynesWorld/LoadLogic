using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Application.Queries.ProductTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    [Collection("DapperSqlite")]
    public class GetProductTypesTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetProductTypesTest()
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
        public async Task GetProductTypes_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetProductTypes();
            var handler = new GetProductTypesHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductTypes_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<ProductType>(context);
            var command = new CreateProductType("AGG", "Aggregate");
            var commandHandler = new CreateProductTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetProductTypes();
            var queryHandler = new GetProductTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("AGG", result.First().Code);
            Assert.Equal("Aggregate", result.First().Description);
        }

        [Fact]
        public async Task GetProductTypes_ShouldReturnMultipleExistingItems()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<ProductType>(context);
            var command = new CreateProductType("AGG", "Aggregate");
            var commandHandler = new CreateProductTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            var command2 = new CreateProductType("LIME", "Hydrated Lime");
            var commandHandler2 = new CreateProductTypeHandler(mediator, repo);
            await commandHandler2.Handle(command2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetProductTypes();
            var queryHandler = new GetProductTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal(2, (int)result.Count());

            var x = result.Where(x => x.Code == "AGG").FirstOrDefault();
            Assert.NotNull(x);
            Assert.Equal("AGG", x!.Code);
            Assert.Equal("Aggregate", x.Description);

            var x2 = result.Where(x => x.Code == "LIME").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("LIME", x2!.Code);
            Assert.Equal("Hydrated Lime", x2.Description);
        }
    }
}
