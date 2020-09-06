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
    public class DeleteVendorTest
    {
        [Fact]
        public async Task DeleteVendor_ShouldDelete()
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

            var command = new CreateVendor(
                "ACME", "ACME Company", null,
                Mock.GetAddress(), Mock.GetAddress(),
                Mock.GetPhoneNumber(), Mock.GetPhoneNumber(),
                "www.acme.com", null, CommunicationMethod.Email,
                true, (decimal)55.00, "Testing note.");

            var handler = new CreateVendorHandler(mediator, vendorRepo, companyTypeRepo, regionRepo);
            var resultId = await handler.Handle(command, default);
            var result = await vendorRepo.FindByIdAsync(resultId);
            Assert.NotNull(result);

            //When
            var deleteRequest = new DeleteVendor(resultId);
            var deleteHandler = new DeleteVendorHandler(mediator, vendorRepo);
            await deleteHandler.Handle(deleteRequest, default);

            //Then
            var result2 = await vendorRepo.FindByIdAsync(resultId);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteVendor_ShouldThrow_WhenVendorIsNotFound()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Vendor>(context);

            //When
            var deleteRequest = new DeleteVendor(999);
            var deleteHandler = new DeleteVendorHandler(mediator, repo);

            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
        }
    }
}
