using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.Regions;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class CreateRegionTest
    {
        [Fact]
        public async Task CreateRegion_ShouldCreate()
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

            // When
            var resultId = await handler.Handle(request, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("SE", result!.Code);
            Assert.Equal("Southeast", result!.Description);
        }

        [Fact]
        public async Task CreateRegion_ShouldThrow_WhenADuplicateCodeExist_WithSameOriginAndBu()
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
            var request2 = new CreateRegion("SE", "SouthEast");
            var handler2 = new CreateRegionHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(request2, default));
        }
    }
}
