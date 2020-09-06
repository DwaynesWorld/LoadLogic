using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Domain;
using LoadLogic.Services.Vendors.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public class CreateCompanyTypeTest
    {
        [Fact]
        public async Task CreateCompanyType_ShouldCreate()
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

            // When
            var resultId = await handler.Handle(request, default);

            // Then
            var result = await repo.FindByIdAsync(resultId);

            Assert.NotNull(result);
            Assert.Equal(resultId, result!.Id);
            Assert.Equal("SUB", result!.Code);
            Assert.Equal("Subcontractor", result!.Description);
        }

        [Fact]
        public async Task CreateCompanyType_ShouldThrow_WhenADuplicateCodeExist()
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
            var request2 = new CreateCompanyType("SUB", "Subs");
            var handler2 = new CreateCompanyTypeHandler(mediator, repo);

            // Then
            await Assert.ThrowsAsync<DuplicateCodeException>(async () => await handler2.Handle(request2, default));
        }
    }
}
