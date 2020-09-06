using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
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
    public class GetMinorityTypeByIdTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetMinorityTypeByIdTest()
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
        public async Task GetMinorityTypeById_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var repo = new Repository<MinorityType>(context);
            var command = new CreateMinorityType("DBE", "Disadvantaged Business Enterprise");
            var commandHandler = new CreateMinorityTypeHandler(mediator, repo);
            var id = await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetMinorityTypeById(id);
            var queryHandler = new GetMinorityTypeByIdHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("DBE", result.Code);
            Assert.Equal("Disadvantaged Business Enterprise", result.Description);
        }

        [Fact]
        public async Task GetMinorityTypeById_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetMinorityTypeById(999);
            var handler = new GetMinorityTypeByIdHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
