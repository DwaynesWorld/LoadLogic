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
    public class UpdateCompanyTypeTest
    {
        [Fact]
        public async Task UpdateCompanyType_ShouldUpdate()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<CompanyType>(context);
            var request = new CreateCompanyType("SUB", "Subcontractor");
            var handler = new CreateCompanyTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateCompanyType(resultId, "SUB", "Subs");
            var updateHandler = new UpdateCompanyTypeHandler(mediator, repo);
            await updateHandler.Handle(update, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("SUB", result!.Code);
            Assert.Equal("Subs", result!.Description);
        }

        [Fact]
        public async Task UpdateCompanyType_ShouldThrow_WhenTypeIsNotFound()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<CompanyType>(context);
            var request = new CreateCompanyType("SUB", "Subcontractor");
            var handler = new CreateCompanyTypeHandler(mediator, repo);
            var resultId = await handler.Handle(request, default);

            // When
            var update = new UpdateCompanyType(9999, "SUB", "Subs");
            var updateHandler = new UpdateCompanyTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<NotFoundException>(async () => await updateHandler.Handle(update, default));
        }

        [Fact]
        public async Task UpdateCompanyType_ShouldThrow_WhenCodeChangesADuplicateCodeExist()
        {
            // Given
            var mediator = new TestMediator();

            var options = new DbContextOptionsBuilder<VendorsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new VendorsContext(options);
            var repo = new Repository<CompanyType>(context);

            {
                var request = new CreateCompanyType("SUB", "Subcontractor");
                var handler = new CreateCompanyTypeHandler(mediator, repo);
                var resultId = await handler.Handle(request, default);
            }

            var request2 = new CreateCompanyType("SUP", "Supplier");
            var handler2 = new CreateCompanyTypeHandler(mediator, repo);
            var resultId2 = await handler2.Handle(request2, default);

            // When
            var request3 = new UpdateCompanyType(resultId2, "SUB", "Supplier Sub");
            var handler3 = new UpdateCompanyTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler3.Handle(request3, default));
        }
    }
}
