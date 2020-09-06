using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Queries.CompanyTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    [Collection("DapperSqlite")]
    public class GetCompanyTypeByIdTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetCompanyTypeByIdTest()
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
        public async Task GetCompanyTypeById_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var repo = new Repository<CompanyType>(context);
            var command = new CreateCompanyType("SUB", "Subcontractor");
            var commandHandler = new CreateCompanyTypeHandler(mediator, repo);
            var id = await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetCompanyTypeById(id);
            var queryHandler = new GetCompanyTypeByIdHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("SUB", result.Code);
            Assert.Equal("Subcontractor", result.Description);
        }

        [Fact]
        public async Task GetCompanyTypeById_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given

            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetCompanyTypeById(999);
            var handler = new GetCompanyTypeByIdHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
