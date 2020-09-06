using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class CreateMinorityTypeTest
    {
        [Fact]
        public async Task CreateMinorityType_ShouldCreate()
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

            // When
            var resultId = await handler.Handle(request, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("DBE", result!.Code);
            Assert.Equal("Disadvantaged Business Enteprise", result!.Description);
        }

        [Fact]
        public async Task CreateMinorityType_ShouldThrow_WhenADuplicateCodeExist()
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

            // When
            var request2 = new CreateMinorityType("DBE", "Disadvantaged Business Enteprises");
            var handler2 = new CreateMinorityTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(request2, default));
        }
    }
}
