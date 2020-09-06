using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
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
    public class GetRegionByIdTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetRegionByIdTest()
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
        public async Task GetRegionById_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();



            using var context = new VendorsContext(_options);
            var repo = new Repository<Region>(context);
            var command = new CreateRegion("SE", "Southeast");
            var commandHandler = new CreateRegionHandler(mediator, repo);
            var id = await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetRegionById(id);
            var queryHandler = new GetRegionByIdHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("SE", result.Code);
            Assert.Equal("Southeast", result.Description);
        }

        [Fact]
        public async Task GetRegionById_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given


            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetRegionById(999);
            var handler = new GetRegionByIdHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
