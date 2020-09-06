using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class CreateProductTypeTest
    {
        [Fact]
        public async Task CreateProductType_ShouldCreate()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<ProductType>(context);

            var request = new CreateProductType("AGG", "Aggregate");
            var handler = new CreateProductTypeHandler(mediator, repo);

            // When
            var resultId = await handler.Handle(request, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("AGG", result!.Code);
            Assert.Equal("Aggregate", result!.Description);
        }

        [Fact]
        public async Task CreateProductType_ShouldThrow_WhenADuplicateCodeExist()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<ProductType>(context);

            {
                var request = new CreateProductType("AGG", "Aggregate");
                var handler = new CreateProductTypeHandler(mediator, repo);
                var resultId = await handler.Handle(request, default);
            }

            // When
            var request2 = new CreateProductType("AGG", "Aggregates");
            var handler2 = new CreateProductTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(request2, default));
        }
    }
}
