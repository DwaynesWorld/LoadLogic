using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.Regions;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class DeleteRegionTest
    {
        [Fact]
        public async Task DeleteRegion_ShouldDelete()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Region>(context);
            var request = new CreateRegion("SE", "Southeast");
            var handler = new CreateRegionHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);
            var result = await repo.FindByIdAsync(resultId);
            Assert.NotNull(result);

            //When
            var deleteRequest = new DeleteRegion(resultId);
            var deleteHandler = new DeleteRegionHandler(mediator, repo);
            await deleteHandler.Handle(deleteRequest, default);

            //Then
            var result2 = await repo.FindByIdAsync(resultId);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteRegion_ShouldThrow_WhenRegionIsNotFound()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Region>(context);

            //When
            var deleteRequest = new DeleteRegion(999);
            var deleteHandler = new DeleteRegionHandler(mediator, repo);
            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
        }
    }
}
