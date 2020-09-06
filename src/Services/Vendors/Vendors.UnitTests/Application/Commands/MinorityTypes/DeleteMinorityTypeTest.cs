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
    public class DeleteMinorityTypeTest
    {
        [Fact]
        public async Task DeleteMinorityType_ShouldDelete()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<MinorityType>(context);
            var request = new CreateMinorityType("DBE", "Disadvantaged Business Enteprise");
            var handler = new CreateMinorityTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);
            var result = await repo.FindByIdAsync(resultId);
            Assert.NotNull(result);

            //When
            var deleteRequest = new DeleteMinorityType(resultId);
            var deleteHandler = new DeleteMinorityTypeHandler(mediator, repo);
            await deleteHandler.Handle(deleteRequest, default);

            //Then
            var result2 = await repo.FindByIdAsync(resultId);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteMinorityType_ShouldThrow_WhenMinorityTypeIsNotFound()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<MinorityType>(context);

            //When
            var deleteRequest = new DeleteMinorityType(999);
            var deleteHandler = new DeleteMinorityTypeHandler(mediator, repo);
            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
        }
    }
}
