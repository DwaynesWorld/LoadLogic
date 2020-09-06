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
    public class DeleteProductTypeTest
    {
        [Fact]
        public async Task DeleteProductType_ShouldDelete()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<ProductType>(context);
            var request = new CreateProductType("AGG", "Aggregate");
            var handler = new CreateProductTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);
            var result = await repo.FindByIdAsync(resultId);
            Assert.NotNull(result);

            //When
            var deleteRequest = new DeleteProductType(resultId);
            var deleteHandler = new DeleteProductTypeHandler(mediator, repo);
            await deleteHandler.Handle(deleteRequest, default);

            //Then
            var result2 = await repo.FindByIdAsync(resultId);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteProductType_ShouldThrow_WhenProductTypeIsNotFound()
        {
            //Given
            var mediator = new TestMediator();



            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<ProductType>(context);

            //When
            var deleteRequest = new DeleteProductType(999);
            var deleteHandler = new DeleteProductTypeHandler(mediator, repo);
            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
        }
    }
}
