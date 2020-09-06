using System;
using System.Linq;
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
    public class UpdateVendorContactTest
    {
        [Fact]
        public async Task UpdateVendorContact_ShouldUpdate()
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

            var createVendorCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var vendorId = await createHandler.Handle(createVendorCommand, default);

            var createVendorContactCommand = new CreateVendorContact(
                vendorId, "Kyle", "Thompson", "Sr Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", true);
            var createContactHandler = new CreateVendorContactHandler(mediator, vendorRepo);
            var contactId = await createContactHandler.Handle(createVendorContactCommand, default);

            //When
            var updateCommand = new UpdateVendorContact(
                vendorId, contactId, "Kyle", "Joe", "VP Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", false);

            var updateHandler = new UpdateVendorContactHandler(mediator, vendorRepo);
            await updateHandler.Handle(updateCommand, default);

            //Then
            var vendor = await vendorRepo.FindByIdAsync(vendorId);

            Assert.NotNull(vendor);
            Assert.Equal("ACME Company", vendor!.Name);
            Assert.Equal("www.acme.com", vendor!.WebAddress);
            Assert.Equal((decimal)55.00, vendor.BondRate);
            Assert.Single(vendor.Contacts);

            var contact = vendor.Contacts.First();
            Assert.Equal("Kyle", contact.FirstName);
            Assert.Equal("Joe", contact.LastName);
            Assert.Equal("VP Program Director", contact.Title);
            Assert.False(contact.IsMainContact);
        }

        [Fact]
        public async Task UpdateVendorContact_ShouldUpdate_WithExistingMainVendors()
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

            var createVendorCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var vendorId = await createHandler.Handle(createVendorCommand, default);

            var createVendorContactCommand = new CreateVendorContact(
                vendorId, "Kyle", "Thompson", "Sr Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", false);
            var createContactHandler = new CreateVendorContactHandler(mediator, vendorRepo);
            var contactId = await createContactHandler.Handle(createVendorContactCommand, default);

            var createVendorContactCommand2 = new CreateVendorContact(
                vendorId, "Joe", "Dohn", "",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", true);
            var createContactHandler2 = new CreateVendorContactHandler(mediator, vendorRepo);
            var contactId2 = await createContactHandler.Handle(createVendorContactCommand2, default);

            //When
            var updateCommand = new UpdateVendorContact(
                vendorId, contactId, "Kyle", "Joe", "VP Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", true);

            var updateHandler = new UpdateVendorContactHandler(mediator, vendorRepo);
            await updateHandler.Handle(updateCommand, default);

            //Then
            var vendor = await vendorRepo.FindByIdAsync(vendorId);

            Assert.NotNull(vendor);
            Assert.Equal("ACME Company", vendor!.Name);
            Assert.Equal("www.acme.com", vendor!.WebAddress);
            Assert.Equal((decimal)55.00, vendor.BondRate);

            var count = vendor.Contacts.Count();
            Assert.Equal(2, count);

            var contact1 = vendor.Contacts.Where(c => c.Id == contactId).Single();
            Assert.Equal("Kyle", contact1.FirstName);
            Assert.Equal("Joe", contact1.LastName);
            Assert.Equal("VP Program Director", contact1.Title);
            Assert.True(contact1.IsMainContact);

            var contact2 = vendor.Contacts.Where(c => c.Id == contactId2).Single();
            Assert.Equal("Joe", contact2.FirstName);
            Assert.Equal("Dohn", contact2.LastName);
            Assert.Equal("", contact2.Title);
            Assert.False(contact2.IsMainContact);
        }

        [Fact]
        public async Task UpdateVendorContact_ShouldThrow_WhenVendorIsNotFound()
        {
            //Given
            var mediator = new TestMediator();



            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var vendorRepo = new Repository<Vendor>(context);

            //When
            var updateCommand = new UpdateVendorContact(
                999, 999, "Kyle", "Joe", "VP Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", false);

            var updateHandler = new UpdateVendorContactHandler(mediator, vendorRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(updateCommand, default));
        }

        [Fact]
        public async Task UpdateVendorContact_ShouldThrow_WhenVendorContactIsNotFound()
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

            var createVendorCommand = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var createHandler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var vendorId = await createHandler.Handle(createVendorCommand, default);

            //When
            var updateCommand = new UpdateVendorContact(
                vendorId, 9999, "Kyle", "Joe", "VP Program Director",
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "k@t.com", "Contact Note", false);

            var updateHandler = new UpdateVendorContactHandler(mediator, vendorRepo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(updateCommand, default));
        }
    }
}
