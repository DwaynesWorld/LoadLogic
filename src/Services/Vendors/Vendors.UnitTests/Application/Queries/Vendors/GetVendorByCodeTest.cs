using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Application.Commands.Regions;
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
    public class GetVendorByCodeTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<VendorsContext> _options;

        public GetVendorByCodeTest()
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
        public async Task GetVendorByCode_ShouldReturnExistingItem()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var primary = Mock.GetAddress();
            var alt = Mock.GetAddress();
            var command = new CreateVendor("V1", "Vendor 1", null, primary, alt, Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.NotNull(result.PrimaryAddress!);
            Assert.Equal(primary.AddressLine1, result.PrimaryAddress!.AddressLine1);
            Assert.NotNull(result.AlternateAddress!);
            Assert.Equal(alt.AddressLine1, result.AlternateAddress!.AddressLine1);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }


        [Fact]
        public async Task GetVendorByCode_ShouldReturnExistingItem_WhenCompanyTypeExist()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var typeCommand = new CreateCompanyType("SUB", "Subcontractor");
            var typeHandler = new CreateCompanyTypeHandler(mediator, compTypeRepo);
            var typeId = await typeHandler.Handle(typeCommand, default);

            var command = new CreateVendor("V1", "Vendor 1", typeId, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", null, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.NotNull(result.Type!);
            Assert.Equal("SUB", result.Type!.Code);
            Assert.Equal("Subcontractor", result.Type!.Description);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }

        [Fact]
        public async Task GetVendorByCode_ShouldReturnExistingItem_WhenRegionExist()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var regionCommand = new CreateRegion("EAST", "East Region");
            var regionHandler = new CreateRegionHandler(mediator, regionRepo);
            var regionId = await regionHandler.Handle(regionCommand, default);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", regionId, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            await commandHandler.Handle(command, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.NotNull(result.Region!);
            Assert.Equal("EAST", result.Region!.Code);
            Assert.Equal("East Region", result.Region!.Description);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }

        [Fact]
        public async Task GetVendorByCode_ShouldReturnExistingItem_WhenMinorityStatusExist()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);
            var minorityTypeRepo = new Repository<MinorityType>(context);

            var regionCommand = new CreateRegion("EAST", "East Region");
            var regionHandler = new CreateRegionHandler(mediator, regionRepo);
            var regionId = await regionHandler.Handle(regionCommand, default);

            var minorityTypeCommand = new CreateMinorityType("MT1", "Test minority type 1");
            var minorityTypeHandler = new CreateMinorityTypeHandler(mediator, minorityTypeRepo);
            var minorityTypeId = await minorityTypeHandler.Handle(minorityTypeCommand, default);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", regionId, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var vendorId = await commandHandler.Handle(command, default);
            var minorityStatusCommand = new CreateVendorMinorityStatus(vendorId, minorityTypeId, "Cert#", 10);
            var minorityStatusHandler = new CreateVendorMinorityStatusHandler(mediator, vendorRepo, minorityTypeRepo);
            await minorityStatusHandler.Handle(minorityStatusCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.NotNull(result.MinorityStatuses!);
            Assert.Equal(result.Id, result.MinorityStatuses![0].CompanyId);
            Assert.Equal("MT1", result.MinorityStatuses![0].Type!.Code);
            Assert.Equal("Test minority type 1", result.MinorityStatuses![0].Type!.Description);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }

        [Fact]
        public async Task GetVendorByCode_ShouldReturnExistingItem_WhenProductsExist()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);
            var productTypeRepo = new Repository<ProductType>(context);

            var regionCommand = new CreateRegion("EAST", "East Region");
            var regionHandler = new CreateRegionHandler(mediator, regionRepo);
            var regionId = await regionHandler.Handle(regionCommand, default);

            var productTypeCommand = new CreateProductType("Product Type 1", "Test product type 1");
            var productTypeHandler = new CreateProductTypeHandler(mediator, productTypeRepo);
            var productTypeId = await productTypeHandler.Handle(productTypeCommand, default);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", regionId, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var vendorId = await commandHandler.Handle(command, default);

            var productCommand = new CreateVendorProduct(vendorId, productTypeId, regionId);
            var productHandler = new CreateVendorProductHandler(mediator, vendorRepo, productTypeRepo, regionRepo);
            await productHandler.Handle(productCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.NotNull(result.Products!);
            Assert.Equal(result.Id, result.Products![0].CompanyId);
            Assert.Equal("PRODUCT TYPE 1", result.Products![0].Product!.Code);
            Assert.Equal("Test product type 1", result.Products![0].Product!.Description);
            Assert.Equal(regionId, result.Products![0].Region!.Id);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }

        [Fact]
        public async Task GetVendorByCode_ShouldReturnExistingItem_WhenVendorsExist()
        {
            //Given
            var mediator = new TestMediator();

            using var context = new VendorsContext(_options);
            var vendorRepo = new Repository<Vendor>(context);
            var compTypeRepo = new Repository<CompanyType>(context);
            var regionRepo = new Repository<Region>(context);

            var regionCommand = new CreateRegion("EAST", "East Region");
            var regionHandler = new CreateRegionHandler(mediator, regionRepo);
            var regionId = await regionHandler.Handle(regionCommand, default);

            var command = new CreateVendor("V1", "Vendor 1", null, Mock.GetAddress(), Mock.GetAddress(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "", regionId, CommunicationMethod.Fax, true, 5, "Note");
            var commandHandler = new CreateVendorHandler(mediator, vendorRepo, compTypeRepo, regionRepo);
            var vendorId = await commandHandler.Handle(command, default);

            var contactCommand = new CreateVendorContact(vendorId, "Firstname", "Lastname", "Title", Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), "test@mail.com", "Note", true);
            var contactHandler = new CreateVendorContactHandler(mediator, vendorRepo);
            await contactHandler.Handle(contactCommand, default);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var queryHandler = new GetVendorByCodeHandler(provider);
            var result = await queryHandler.Handle(query, default);

            //Then
            Assert.NotNull(result);
            Assert.Equal("V1", result.Code);
            Assert.Equal("Vendor 1", result.Name);
            Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
            Assert.NotNull(result.Vendors!);
            Assert.Equal("Firstname", result.Vendors![0].FirstName!);
            Assert.Equal("Lastname", result.Vendors![0].LastName!);
            Assert.Equal("Title", result.Vendors![0].Title!);
            Assert.True(result.IsBonded);
            Assert.Equal(5, result.BondRate);
            Assert.Equal("Note", result.Note);
        }


        [Fact]
        public async Task GetVendorByCode_ShouldThrowNotFound_WhenItemDoesNotExist()
        {
            //Given
            using var context = new VendorsContext(_options);

            // When
            var provider = new TestDbConnectionProvider(context);
            var query = new GetVendorByCode("V1");
            var handler = new GetVendorByCodeHandler(provider);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
        }
    }
}
