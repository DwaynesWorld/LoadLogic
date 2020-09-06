using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
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
    public class GetDetailedVendorsTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetDetailedVendorsTest()
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
        public async Task GetDetailedVendors_ShouldReturnEmpty_WhenNoItemsExist()
        {
            //Given
            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetDetailedVendors();
            var handler = new GetDetailedVendorsHandler(provider);
            var result = await handler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDetailedVendors_ShouldReturnExistingItem()
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
            var query = new GetDetailedVendors();
            var queryHandler = new GetDetailedVendorsHandler(provider);
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
        public async Task GetDetailedVendors_ShouldReturnExistingItem_WithContact()
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
            var id = await commandHandler.Handle(command, default);

            var contactCommand = new CreateVendorContact(id, "First", "Last", "Title", Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), "test@test.com", "Note", true);
            var contactCommandHandler = new CreateVendorContactHandler(mediator, vendorRepo);
            await contactCommandHandler.Handle(contactCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetDetailedVendors();
            var queryHandler = new GetDetailedVendorsHandler(provider);
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

            Assert.NotNull(v.Contacts);
            Assert.Single(v.Contacts);

            var c = v.Contacts.First();
            Assert.Equal("First", c.FirstName);
            Assert.Equal("Last", c.LastName);
            Assert.Equal("test@test.com", c.EmailAddress);
            Assert.Equal("Note", c.Note);
            Assert.True(c.IsMainContact);
        }

        [Fact]
        public async Task GetDetailedVendors_ShouldReturnExistingItem_WithProduct()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);
            var productTypeRepo = new Repository<ProductType>(context);

            var primaryAddress = Mock.GetAddress();
            var altAddress = Mock.GetAddress();
            var phone = Mock.GetPhoneNumber();
            var fax = Mock.GetPhoneNumber();

            var command = new CreateVendor("V1", "Vendor 1", null, primaryAddress, altAddress, phone, fax, "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var id = await commandHandler.Handle(command, default);

            var request = new CreateProductType("AGG", "Aggregate");
            var handler = new CreateProductTypeHandler(mediator, productTypeRepo);
            var typeId = await handler.Handle(request, default);
            var vendProductCommand = new CreateVendorProduct(id, typeId, null);
            var vendProductCommandHandler = new CreateVendorProductHandler(mediator, vendorRepo, productTypeRepo, regionRepo);
            await vendProductCommandHandler.Handle(vendProductCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetDetailedVendors();
            var queryHandler = new GetDetailedVendorsHandler(provider);
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

            Assert.NotNull(v.Products);
            Assert.Single(v.Products);

            var p = v.Products.First();
            Assert.Equal("AGG", p.Product!.Code);
            Assert.Equal("Aggregate", p.Product.Description);
            Assert.Null(p.Region);
        }

        [Fact]
        public async Task GetDetailedVendors_ShouldReturnExistingItem_WithMinorityStatus()
        {
            //Given
            var mediator = new TestMediator();



            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);
            var minTypeRepo = new Repository<MinorityType>(context);

            var primaryAddress = Mock.GetAddress();
            var altAddress = Mock.GetAddress();
            var phone = Mock.GetPhoneNumber();
            var fax = Mock.GetPhoneNumber();

            var command = new CreateVendor("V1", "Vendor 1", null, primaryAddress, altAddress, phone, fax, "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var id = await commandHandler.Handle(command, default);

            var request = new CreateMinorityType("DBE", "Disadvantaged Business Enterprise");
            var handler = new CreateMinorityTypeHandler(mediator, minTypeRepo);
            var typeId = await handler.Handle(request, default);
            var vendMinCommand = new CreateVendorMinorityStatus(id, typeId, "111000111", 50);
            var vendMinCommandHandler = new CreateVendorMinorityStatusHandler(mediator, vendorRepo, minTypeRepo);
            await vendMinCommandHandler.Handle(vendMinCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetDetailedVendors();
            var queryHandler = new GetDetailedVendorsHandler(provider);
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

            Assert.NotNull(v.MinorityStatuses);
            Assert.Single(v.MinorityStatuses);

            var m = v.MinorityStatuses.First();
            Assert.Equal("DBE", m.Type!.Code);
            Assert.Equal("Disadvantaged Business Enterprise", m.Type.Description);
            Assert.Equal("111000111", m.CertificationNumber);
            Assert.Equal(50, m.Percent);
        }

        [Fact]
        public async Task GetDetailedVendors_ShouldReturnMultipleExistingItems()
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

            var contactCommand = new CreateVendorContact(id, "First", "Last", "Title", Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), "test@test.com", "Note", true);
            var contactCommandHandler = new CreateVendorContactHandler(mediator, vendorRepo);
            await contactCommandHandler.Handle(contactCommand, default);

            var command2 = new CreateVendor("V2", "Vendor 2", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Email, true, 52, "Note 2");
            var commandHandler2 = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var id2 = await commandHandler2.Handle(command2, default);

            var contactCommand2 = new CreateVendorContact(id2, "First2", "Last2", "Title2", Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), Mock.GetPhoneNumber().ToString(), "test2@test.com", "Note2", true);
            var contactCommandHandler2 = new CreateVendorContactHandler(mediator, vendorRepo);
            await contactCommandHandler2.Handle(contactCommand2, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetDetailedVendors();
            var queryHandler = new GetDetailedVendorsHandler(provider);
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

            Assert.NotNull(x1.Contacts);
            Assert.Single(x1.Contacts);

            var c = x1.Contacts.First();
            Assert.Equal("First", c.FirstName);
            Assert.Equal("Last", c.LastName);
            Assert.Equal("test@test.com", c.EmailAddress);
            Assert.Equal("Note", c.Note);
            Assert.True(c.IsMainContact);

            var x2 = result.Where(x => x.Code == "V2").FirstOrDefault();
            Assert.NotNull(x2);
            Assert.Equal("V2", x2!.Code);
            Assert.Equal("Vendor 2", x2.Name);
            Assert.Equal(CommunicationMethod.Email, x2.CommunicationMethod);
            Assert.True(x2.IsBonded);
            Assert.Equal(52, x2.BondRate);
            Assert.Equal("Note 2", x2.Note);

            Assert.NotNull(x2.Contacts);
            Assert.Single(x2.Contacts);

            var c2 = x2.Contacts.First();
            Assert.Equal("First2", c2.FirstName);
            Assert.Equal("Last2", c2.LastName);
            Assert.Equal("test2@test.com", c2.EmailAddress);
            Assert.Equal("Note2", c2.Note);
            Assert.True(c.IsMainContact);
        }
    }
}
