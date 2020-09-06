using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class UpdateProductTypeTest
    {
        [Fact]
        public async Task UpdateProductType_ShouldUpdate()
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
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateProductType(resultId, "AGG", "Heavy Aggregate");
            var updateHandler = new UpdateProductTypeHandler(mediator, repo);
            await updateHandler.Handle(update, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("AGG", result!.Code);
            Assert.Equal("Heavy Aggregate", result!.Description);
        }

        [Fact]
        public async Task UpdateProductType_ShouldThrow_WhenTypeIsNotFound()
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
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateProductType(999, "AGG", "Heavy Aggregate");
            var updateHandler = new UpdateProductTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(update, default));
        }

        [Fact]
        public async Task UpdateProductType_ShouldThrow_WhenCodeChangesADuplicateCodeExist()
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

            var request2 = new CreateProductType("MAGG", "Marine Aggregate");
            var handler2 = new CreateProductTypeHandler(mediator, repo);
            var resultId2 = await handler2.Handle(request2, default);

            // When
            var request3 = new UpdateProductType(resultId2, "AGG", "Marine Aggregate");
            var handler3 = new UpdateProductTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler3.Handle(request3, default));
        }
    }
}
