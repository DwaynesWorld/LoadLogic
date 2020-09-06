using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.Vendors;
using LoadLogic.Services.Vendors.Application.Queries.Vendors;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    [Collection("DapperSqlite")]
    public class GetVendorByIdTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetVendorByIdTest()
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
        public async Task GetVendorById_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var id = await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorById(id);
            var queryHandler = new GetVendorByIdHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }

        [Fact]
        public async Task GetVendorById_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given
            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorById(999);
            var handler = new GetVendorByIdHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
