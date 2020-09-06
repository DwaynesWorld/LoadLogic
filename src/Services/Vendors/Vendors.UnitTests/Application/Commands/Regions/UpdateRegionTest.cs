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
    public class UpdateRegionTest
    {
        [Fact]
        public async Task UpdateRegion_ShouldUpdate()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Region>(context);

            var request = new CreateRegion("SE", "Southeast");
            var handler = new CreateRegionHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateRegion(resultId, "SE", "Southeast Region");
            var updateHandler = new UpdateRegionHandler(mediator, repo);
            await updateHandler.Handle(update, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("SE", result!.Code);
            Assert.Equal("Southeast Region", result!.Description);
        }

        [Fact]
        public async Task UpdateRegion_ShouldThrow_WhenTypeIsNotFound()
        {
            // Given
            var mediator = new TestMediator();



            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Region>(context);
            var request = new CreateRegion("SE", "Southeast");
            var handler = new CreateRegionHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateRegion(999, "SE", "Southeast Region");
            var updateHandler = new UpdateRegionHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(update, default));
        }

        [Fact]
        public async Task UpdateRegion_ShouldThrow_WhenCodeChangesADuplicateCodeExist()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<Region>(context);

            var request = new CreateRegion("SE", "Southeast");
            var handler = new CreateRegionHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            request = new CreateRegion("SR", "Southern Region");
            handler = new CreateRegionHandler(mediator, repo);
            resultId = await handler.Handle(request, default);

            // When
            var request2 = new UpdateRegion(resultId, "SE", "South East Region");
            var handler2 = new UpdateRegionHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(request2, default));
        }
    }
}
