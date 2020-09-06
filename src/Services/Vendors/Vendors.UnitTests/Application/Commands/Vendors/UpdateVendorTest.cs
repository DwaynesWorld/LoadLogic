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
    public class UpdateVendorTest
    {
        [Fact]
        public async Task UpdateVendor_ShouldUpdate()
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

            var createCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await createHandler.Handle(createCommand, default);

            //When
            var updateCommand = new UpdateVendor(
                id, "ACME", "ACME Company, LTD", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.net", null, CommunicationMethod.Email,
                true, (decimal)15.00, "Testing note.");

            var updateHandler = new UpdateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            await updateHandler.Handle(updateCommand, default);

            //Then
            var result = await vendorRepo.FindByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("ACME Company, LTD", result!.Name);
            Assert.Equal("www.acme.net", result!.WebAddress);
            Assert.Equal((decimal)15.00, result.BondRate);
        }

        [Fact]
        public async Task UpdateVendor_ShouldThrow_WhenVendorIsNotFound()
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
            var updateCommand = new UpdateVendor(
                9999, "ACME", "ACME Company, LTD", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.net", null, CommunicationMethod.Email,
                true, (decimal)15.00, "Testing note.");

            var updateHandler = new UpdateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(updateCommand, default));
        }

        [Fact]
        public async Task UpdateVendor_ShouldThrow_WhenCompanyTypeIsNotFound()
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

            var createCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await createHandler.Handle(createCommand, default);

            //When
            var updateCommand = new UpdateVendor(
                id, "ACME", "ACME Company, LTD", 9999,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.net", null, CommunicationMethod.Email,
                true, (decimal)15.00, "Testing note.");

            var updateHandler = new UpdateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(updateCommand, default));
        }

        [Fact]
        public async Task UpdateVendor_ShouldThrow_WhenRegionIsNotFound()
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

            var createCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await createHandler.Handle(createCommand, default);

            //When
            var updateCommand = new UpdateVendor(
                id, "ACME", "ACME Company, LTD", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.net", 9999, CommunicationMethod.Email,
                true, (decimal)15.00, "Testing note.");

            var updateHandler = new UpdateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(updateCommand, default));
        }

        [Fact]
        public async Task UpdateVendor_ShouldThrow_WhenCodeChangesAndADuplicateCodeExist()
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
                var createCommand1 = new CreateVendor(
                   "ACME", "ACME Company", null,
                   Mock.GetAddress(), Mock.GetAddress(),
                   Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                   "www.acme.com", null, CommunicationMethod.Email,
                   true, (decimal)55.00, "Testing note.");

                var createHandler1 = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
                await createHandler1.Handle(createCommand1, default);
            }

            var createCommand2 = new CreateVendor(
                "ACC", "ACME Construction Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler2 = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var id = await createHandler2.Handle(createCommand2, default);

            //When
            var updateCommand = new UpdateVendor(
                id, "ACME", "ACME Construction Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.net", null, CommunicationMethod.Email,
                true, (decimal)15.00, "Testing note.");

            var updateHandler = new UpdateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);

            //Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await updateHandler.Handle(updateCommand, default));
        }
    }
}
