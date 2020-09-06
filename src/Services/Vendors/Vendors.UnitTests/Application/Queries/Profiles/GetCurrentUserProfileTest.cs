// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using LoadLogic.Services.Exceptions;
// using LoadLogic.Services.Vendors.Application.Commands.Profiles;
// using LoadLogic.Services.Vendors.Application.Queries.Profiles;
// using LoadLogic.Services.Vendors.Domain;
// using LoadLogic.Services.Vendors.Infrastructure.Persistence;

// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests
// {
//     [Collection("DapperSqlite")]
//     public class GetCurrentUserProfileTest : IDisposable
//     {
//         private readonly SqliteConnection _connection;
//         private readonly DbContextOptions<VendorsContext> _options;

//         public GetCurrentUserProfileTest()
//         {
//             _connection = new SqliteConnection("DataSource=:memory:");
//             _connection.Open();

//             _options = new DbContextOptionsBuilder<VendorsContext>().UseSqlite(_connection).Options;
//             using var context = new VendorsContext(_options);
//             context.Database.EnsureCreated();
//         }

//         public void Dispose()
//         {
//             _connection.Close();
//         }

//         [Fact]
//         public async Task GetCurrentUserProfile_ShouldThrowNotFound_WhenNoProfileExistsForCompanyAndBusinessUnit()
//         {
//             //Given
//             using var context = new VendorsContext(_options);

//             // When
//             var provider = new TestDbConnectionProvider(context);
//             var query = new GetCurrentUserProfile();
//             var handler = new GetCurrentUserProfileHandler(provider);

//             //Then
//             await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, default));
//         }

//         [Fact]
//         public async Task GetCurrentUserProfile_ShouldReturnExistingItem()
//         {
//             // Given
//             var mediator = new TestMediator();


//             using var context = new VendorsContext(_options);
//             var profileRepo = new Repository<Profile>(context);
//             var compTypeRepo = new Repository<CompanyType>(context);
//             var regionRepo = new Repository<Region>(context);

//             var primaryAddress = Mock.GetAddress();
//             var altAddress = Mock.GetAddress();
//             var phone = Mock.GetPhoneNumber();
//             var fax = Mock.GetPhoneNumber();

//             var command = new CreateProfile("HCSS", primaryAddress, altAddress, phone, fax, "", null, CommunicationMethod.Fax, "");
//             var commandHandler = new CreateProfileHandler(mediator, profileRepo, regionRepo);
//             var id = await commandHandler.Handle(command, default);

//             // When
//             var provider = new TestDbConnectionProvider(context);
//             var query = new GetCurrentUserProfile();
//             var handler = new GetCurrentUserProfileHandler(provider);
//             var result = await handler.Handle(query, default);

//             // Then
//             Assert.NotNull(result);
//             Assert.Equal("HCSS", result.Name);
//             Assert.Equal(primaryAddress, result.PrimaryAddress);
//             Assert.Equal(altAddress, result.AlternateAddress);
//             Assert.Equal(phone.ToString(), result.PhoneNumber);
//             Assert.Equal(fax.ToString(), result.FaxNumber);
//             Assert.Equal(CommunicationMethod.Fax, result.CommunicationMethod);
//         }
//     }
// }
