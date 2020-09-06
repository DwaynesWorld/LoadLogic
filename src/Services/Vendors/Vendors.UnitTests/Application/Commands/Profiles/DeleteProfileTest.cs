// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Bogus;
// using LoadLogic.Services;
// using LoadLogic.Services.Exceptions;
// using LoadLogic.Services.Vendors.Application.Commands.Profiles;
// using LoadLogic.Services.Vendors.Domain;
// using LoadLogic.Services.Vendors.Infrastructure.Persistence;

// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests
// {
//     public class DeleteProfileTest
//     {
//         [Fact]
//         public async Task DeleteProfile_ShouldDelete()
//         {
//             // Given
//             var mediator = new TestMediator();

//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);

//             var profile = GetProfile();
//             var request = new CreateProfile(
//                 profile.Name, profile.PrimaryAddress, profile.AlternateAddress,
//                 profile.PhoneNumber, profile.FaxNumber, profile.WebAddress,
//                 profile.RegionId, profile.CommunicationMethod, profile.ProfileAccentColor);
//             var handler = new CreateProfileHandler(mediator, repo, regionRepo);

//             // When
//             var resultId = await handler.Handle(request, default);
//             var result = await repo.FindByIdAsync(resultId);
//             Assert.NotNull(result);

//             var deleteRequest = new DeleteProfile();
//             var deleteHandler = new DeleteProfileHandler(mediator, repo);
//             await deleteHandler.Handle(deleteRequest, default);
//             // Then
//             var result2 = await repo.FindByIdAsync(resultId);
//             Assert.Null(result2);
//         }

//         [Fact]
//         public async Task DeleteProfile_ShouldThrow_WhenAProfileDoesNotExist()
//         {
//             // Given
//             var mediator = new TestMediator();

//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);

//             // When
//             var deleteRequest = new DeleteProfile();
//             var deleteHandler = new DeleteProfileHandler(mediator, repo);

//             // Then
//             await Assert.ThrowsAsync<NotFoundException>(async () => await deleteHandler.Handle(deleteRequest, default));
//         }

//         private static Profile GetProfile()
//         {
//             var faker = new Faker("en");
//             var name = faker.Company.CompanyName();
//             var primaryAddress = new Address(
//                 faker.Address.StreetAddress(),
//                 faker.Address.SecondaryAddress(),
//                 faker.Address.BuildingNumber(),
//                 faker.Address.City(),
//                 faker.Address.State(),
//                 faker.Address.Country(),
//                 faker.Address.ZipCode());
//             var alternateAddress = new Address(
//                 faker.Address.StreetAddress(),
//                 faker.Address.SecondaryAddress(),
//                 faker.Address.BuildingNumber(),
//                 faker.Address.City(),
//                 faker.Address.State(),
//                 faker.Address.Country(),
//                 faker.Address.ZipCode());
//             var phoneNumber = (PhoneNumber)faker.Phone.PhoneNumber("##########");
//             var faxNumber = (PhoneNumber)faker.Phone.PhoneNumber("##########");
//             var webAddress = faker.Internet.Url();
//             var communicationMethod = CommunicationMethod.Email;
//             var profileAccentColor = "#EEE";

//             return new Profile(
//                 name, primaryAddress, alternateAddress,
//                 phoneNumber, faxNumber,
//                 webAddress, null, communicationMethod, profileAccentColor);
//         }
//     }
// }
