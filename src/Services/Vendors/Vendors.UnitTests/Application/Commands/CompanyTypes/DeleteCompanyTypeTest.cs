using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class DeleteCompanyTypeTest
    {
        [Fact]
        public async Task DeleteCompanyType_ShouldDelete()
        {
            //Given
            var mediator = new TestMediator();


            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<CompanyType>(context);
            var request = new CreateCompanyType("SUB", "Subcontractor");
            var handler = new CreateCompanyTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);
            var result = await repo.FindByIdAsync(resultId);
            Assert.NotNull(result);

            //When
            var deleteRequest = new DeleteCompanyType(resultId);
            var deleteHandler = new DeleteCompanyTypeHandler(mediator, repo);
            await deleteHandler.Handle(deleteRequest, default);

            //Then
            var result2 = await repo.FindByIdAsync(resultId);
            Assert.Null(result2);
        }

        [Fact]
        public async Task DeleteCompanyType_ShouldThrow_WhenCompanyTypeIsNotFound()
        {
            //Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<CompanyType>(context);

            //When
            var deleteRequest = new DeleteCompanyType(1000);
            var deleteHandler = new DeleteCompanyTypeHandler(mediator, repo);
            //Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
        }
    }
}
