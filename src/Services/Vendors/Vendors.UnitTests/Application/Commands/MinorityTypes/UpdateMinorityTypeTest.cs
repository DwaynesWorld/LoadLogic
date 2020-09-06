using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class UpdateMinorityTypeTest
    {
        [Fact]
        public async Task UpdateMinorityType_ShouldUpdate()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<MinorityType>(context);
            var request = new CreateMinorityType("DBE", "Disadvantaged Business Enteprise");
            var handler = new CreateMinorityTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateMinorityType(resultId, "DBE", "Disadvantaged Businesses");
            var updateHandler = new UpdateMinorityTypeHandler(mediator, repo);
            await updateHandler.Handle(update, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("DBE", result!.Code);
            Assert.Equal("Disadvantaged Businesses", result!.Description);
        }

        [Fact]
        public async Task UpdateMinorityType_ShouldThrow_WhenTypeIsNotFound()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<MinorityType>(context);

            {
                var request = new CreateMinorityType("SUB", "Disadvantaged Business Enteprise");
                var handler = new CreateMinorityTypeHandler(mediator, repo);
                var resultId = await handler.Handle(request, default);
            }

            // When
            var update = new UpdateMinorityType(9999, "DBE", "Disadvantaged Businesses");
            var updateHandler = new UpdateMinorityTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(update, default));
        }

        [Fact]
        public async Task UpdateMinorityType_ShouldThrow_WhenCodeChangesADuplicateCodeExist()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<MinorityType>(context);

            {
                var request = new CreateMinorityType("DBE", "Disadvantaged Business Enteprise");
                var handler = new CreateMinorityTypeHandler(mediator, repo);
                var resultId = await handler.Handle(request, default);
            }

            var request2 = new CreateMinorityType("MBE", "Minority Businesses");
            var handler2 = new CreateMinorityTypeHandler(mediator, repo);
            var resultId2 = await handler2.Handle(request2, default);

            // When
            var request3 = new UpdateMinorityType(resultId2, "DBE", "Disadvantaged Minority");
            var handler3 = new UpdateMinorityTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler3.Handle(request3, default));
        }
    }
}
