// using System;
// using System.Threading.Tasks;
// using Bogus;
// using LoadLogic.Services;
// using LoadLogic.Services.Exceptions;
// using LoadLogic.Services.Vendors.Application.Commands.Profiles;
// using LoadLogic.Services.Vendors.Domain;
// using LoadLogic.Services.Vendors.Infrastructure.Persistence;

// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests
// {
//     public class UpdateProfileTest
//     {
//         [Fact]
//         public async Task UpdateProfile_ShouldUpdate()
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
//             var id = await handler.Handle(request, default);
//             var result = await repo.FindByIdAsync(id);

//             // When
//             var request2 = new UpdateProfile(
//                 "New Name", profile.PrimaryAddress, profile.AlternateAddress,
//                 profile.PhoneNumber, profile.FaxNumber, profile.WebAddress,
//                 profile.RegionId, profile.CommunicationMethod, "#123123");
//             var handler2 = new UpdateProfileHandler(mediator, repo, regionRepo);
//             await handler2.Handle(request2, default);
//             var updated = await repo.FindByIdAsync(id);

//             // Then
//             Assert.NotNull(updated);
//             Assert.Null(updated!.Region);
//             Assert.Equal("New Name", updated.Name);
//             Assert.Equal(profile.FaxNumber, updated.FaxNumber);
//             Assert.Equal(profile.WebAddress, updated.WebAddress);
//             Assert.Equal("#123123", updated.ProfileAccentColor);
//         }

//         [Fact]
//         public async Task UpdateProfile_ShouldThrow_WhenAProfileDoesNotExist()
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

//             // When
//             var request = new UpdateProfile(
//                 "New Name", profile.PrimaryAddress, profile.AlternateAddress,
//                 profile.PhoneNumber, profile.FaxNumber, profile.WebAddress,
//                 profile.RegionId, profile.CommunicationMethod, "#123123");
//             var handler = new UpdateProfileHandler(mediator, repo, regionRepo);

//             // Then
//             await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, default));
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
//                 name,
//                 primaryAddress, alternateAddress,
//                 phoneNumber, faxNumber,
//                 webAddress, null, communicationMethod, profileAccentColor);
//         }
//     }
// }
