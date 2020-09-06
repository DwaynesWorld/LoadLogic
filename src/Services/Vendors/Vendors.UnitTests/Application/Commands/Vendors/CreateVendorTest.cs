using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.Vendors;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class CreateVendorTest
    {
        [Fact]
        public async Task CreateVendor_ShouldCreate()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            //When
            var a1 = Mock.GetAddress();
            var a2 = Mock.GetAddress();
            var phone = Mock.GetPhoneNumber();
            var fax = Mock.GetPhoneNumber();

            var command = new CreateVendor(
                "ACME", "ACME Company", null,
                a1, a2, phone, fax, "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await handler.Handle(command, default);

            //Then
            var result = await vendorRepo.FindByIdAsync(id);

            Assert.NotNull(result);
            Assert.IsType<long>(result!.Id);
            Assert.Equal(id, result!.Id);
            Assert.Equal("ACME", result!.Code);
            Assert.Equal("ACME Company", result!.Name);
            Assert.Null(result.Type);
            Assert.Equal(a1, result.PrimaryAddress);
            Assert.Equal(a2, result.AlternateAddress);
            Assert.Equal(phone, result.PhoneNumber);
            Assert.Equal("www.acme.com", result.WebAddress);
            Assert.Null(result.Region);
            Assert.Equal(CommunicationMethod.Email, result.CommunicationMethod);
            Assert.True(result.IsBonded);
            Assert.Equal((decimal)55.00, result.BondRate);
            Assert.Equal("Testing note.", result.Note);
        }

        [Fact]
        public async Task CreateVendor_ShouldCreate_WithCompanyType()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            var type = new CompanyType("SUB", "Subcontractor");
            companyTypeRepo.Add(type);
            await companyTypeRepo.UnitOfWork.SaveEntitiesAsync();

            //When
            var command = new CreateVendor(
                "ACME", "ACME Company", type.Id,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await handler.Handle(command, default);

            //Then
            var result = await vendorRepo.FindByIdAsync(id, Vendor.IncludeCompanyType);

            Assert.NotNull(result);
            Assert.NotNull(result!.Type);
        }

        [Fact]
        public async Task CreateVendor_ShouldCreate_WithRegion()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            var region = new Region("SE", "Southeast");
            regionRepo.Add(region);
            await regionRepo.UnitOfWork.SaveEntitiesAsync();

            //When
            var command = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", region.Id, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await handler.Handle(command, default);

            //Then
            var result = await vendorRepo.FindByIdAsync(id, Vendor.IncludeRegion);

            Assert.NotNull(result);
            Assert.NotNull(result!.Region);
        }

        [Fact]
        public async Task CreateVendor_ShouldThrow_WhenDuplicateCodeExist()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            {
                var command1 = new CreateVendor(
                   "ACME", "ACME Company", null,
                   Mock.GetAddress(), Mock.GetAddress(),
                   Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                   "www.acme.com", null, CommunicationMethod.Email,
                   true, (decimal)55.00, "Testing note.");

                var handler1 = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
                await handler1.Handle(command1, default);
            }

            //When
            var command2 = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler2 = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(command2, default));
        }

        [Fact]
        public async Task CreateVendor_ShouldThrow_WhenCompanyTypeDoesNotExist()
        {
            //Given
            var mediator = new TestMediator();



            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            //When
            var command = new CreateVendor(
                "ACME", "ACME Company", 9999,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }

        [Fact]
        public async Task CreateVendor_ShouldThrow_WhenRegionDoesNotExist()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new VendorsContext(options);

            var vendorRepo = new Repository<Vendor>(context);
            var regionRepo = new Repository<Region>(context);
            var companyTypeRepo = new Repository<CompanyType>(context);

            //When
            var command = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", 999, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, default));
        }
    }
}
