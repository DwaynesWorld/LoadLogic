using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class GetVendorsTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetVendorsTest()
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
        public async Task GetVendors_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given
            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendors();
            var handler = new GetVendorsHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetVendors_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var primaryAddress = Mock.GetAddress();
            var altAddress = Mock.GetAddress();
            var phone = Mock.GetPhoneNumber();
            var fax = Mock.GetPhoneNumber();

            var command = new CreateVendor("V1", "Vendor 1", null, primaryAddress, altAddress, phone, fax, "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendors();
            var queryHandler = new GetVendorsHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Single(result);

            var v = result.First();
            Assert.Equal("V1", v.Code);
            Assert.Equal("Vendor 1", v.Name);
            Assert.Equal(primaryAddress, v.PrimaryAddress);
            Assert.Equal(phone.ToString(), v.PhoneNumber);
            Assert.Equal(CommunicationMethod.Fax, v.CommunicationMethod);
            Assert.True(v.IsBonded);
            Assert.Equal(5, v.BondRate);
            Assert.Equal("Note", v.Note);
        }

        [Fact]
        public async Task GetVendors_ShouldReturnMultipleExistingItems()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler.Handle(command, default);

            var command2 = new CreateVendor("V2", "Vendor 2", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Email, true, 52, "Note 2");
            var commandHandler2 = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler2.Handle(command2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendors();
            var queryHandler = new GetVendorsHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal(2, (int)result.Count());

            var x1 = result.Where(x => x.Code == "V1").FirstOrDefault();
            Assert.NotNull(x1);
            Assert.Equal("V1", x1!.Code);
            Assert.Equal("Vendor 1", x1.Name);
            Assert.Equal(CommunicationMethod.Fax, x1.CommunicationMethod);
            Assert.True(x1.IsBonded);
            Assert.Equal(5, x1.BondRate);
            Assert.Equal("Note", x1.Note);

            var x2 = result.Where(x => x.Code == "V2").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("V2", x2!.Code);
            Assert.Equal("Vendor 2", x2.Name);
            Assert.Equal(CommunicationMethod.Email, x2.CommunicationMethod);
            Assert.True(x2.IsBonded);
            Assert.Equal(52, x2.BondRate);
            Assert.Equal("Note 2", x2.Note);
        }
    }
}
