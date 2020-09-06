using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class GetCompanyTypesTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetCompanyTypesTest()
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
        public async Task GetCompanyTypes_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given
            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetCompanyTypes();
            var handler = new GetCompanyTypesHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCompanyTypes_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var repo = new Repository<CompanyType>(context);
            var command = new CreateCompanyType("SUB", "Subcontractor");
            var commandHandler = new CreateCompanyTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetCompanyTypes();
            var queryHandler = new GetCompanyTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("SUB", result.First().Code);
            Assert.Equal("Subcontractor", result.First().Description);
        }

        [Fact]
        public async Task GetCompanyTypes_ShouldReturnMultipleExistingItems()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var repo = new Repository<CompanyType>(context);
            var command = new CreateCompanyType("SUB", "Subcontractor");
            var commandHandler = new CreateCompanyTypeHandler(mediator, repo);
            await commandHandler.Handle(command, default);

            var command2 = new CreateCompanyType("OWN", "Owner");
            var commandHandler2 = new CreateCompanyTypeHandler(mediator, repo);
            await commandHandler2.Handle(command2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetCompanyTypes();
            var queryHandler = new GetCompanyTypesHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal(2, (int)result.Count());

            var x = result.Where(x => x.Code == "SUB").FirstOrDefault();
            Assert.NotNull(x);
            Assert.Equal("SUB", x!.Code);
            Assert.Equal("Subcontractor", x.Description);

            var x2 = result.Where(x => x.Code == "OWN").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("OWN", x2!.Code);
            Assert.Equal("Owner", x2.Description);
        }
    }
}
