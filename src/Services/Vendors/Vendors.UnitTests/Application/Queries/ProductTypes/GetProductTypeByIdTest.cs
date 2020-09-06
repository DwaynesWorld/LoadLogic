using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
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
    public class GetProductTypeByIdTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetProductTypeByIdTest()
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
        public async Task GetProductTypeById_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();


            using var context = new VendorsContext(_options);
            var repo = new Repository<ProductType>(context);
            var command = new CreateProductType("AGG", "Aggregate");
            var commandHandler = new CreateProductTypeHandler(mediator, repo);
            var id = await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetProductTypeById(id);
            var queryHandler = new GetProductTypeByIdHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("AGG", result.Code);
            Assert.Equal("Aggregate", result.Description);
        }

        [Fact]
        public async Task GetProductTypeById_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetProductTypeById(999);
            var handler = new GetProductTypeByIdHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
