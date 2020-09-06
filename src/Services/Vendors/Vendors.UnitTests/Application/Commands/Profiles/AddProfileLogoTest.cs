// using System;
// using System.IO;
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
//     public class AddProfileLogoTest
//     {
//         [Fact]
//         public async Task ShouldAdd()
//         {
//             //Given
//             var mediator = new TestMediator();



//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);
//             var service = new TestBlobService();
//             var id = CreateProfile(mediator, repo, regionRepo);

//             // When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo1_Good.png");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("image/png", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);
//             var url = await handler.Handle(request, default);

//             //Then
//             Assert.False(string.IsNullOrWhiteSpace(url));
//         }

//         [Fact]
//         public async Task ShouldThrow_WhenProfileDoesNotExist()
//         {
//             //Given
//             //Given
//             var mediator = new TestMediator();



//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var service = new TestBlobService();

//             //When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo1_Good.png");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("image/png", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);

//             //Then
//             await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, default));
//         }

//         [Fact]
//         public async Task ShouldThrow_WhenContentTypeIsUnsupported()
//         {
//             //Given
//             var mediator = new TestMediator();



//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);
//             var service = new TestBlobService();
//             var id = CreateProfile(mediator, repo, regionRepo);

//             // When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo2_Type.pdf");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("application/pdf", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);

//             //Then
//             await Assert.ThrowsAsync<InvalidImageFormatException>(async () => await handler.Handle(request, default));
//         }

//         [Fact]
//         public async Task ShouldThrow_WhenContentIsUnsupported()
//         {
//             //Given
//             var mediator = new TestMediator();



//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);
//             var service = new TestBlobService();
//             var id = CreateProfile(mediator, repo, regionRepo);

//             // When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo2_Type.pdf");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("image/pdf", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);

//             //Then
//             await Assert.ThrowsAsync<InvalidImageFormatException>(async () => await handler.Handle(request, default));
//         }

//         [Fact]
//         public async Task ShouldThrow_WhenImageIsNotValid()
//         {
//             //Given
//             var mediator = new TestMediator();



//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);
//             var service = new TestBlobService();
//             var id = CreateProfile(mediator, repo, regionRepo);

//             // When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo3_Corrupt.jpeg");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("image/jpeg", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);

//             //Then
//             await Assert.ThrowsAsync<InvalidImageFormatException>(async () => await handler.Handle(request, default));
//         }


//         [Fact]
//         public async Task ShouldThrow_WhenImageIsTooBig()
//         {
//             //Given
//             var mediator = new TestMediator();

//             var options = new DbContextOptionsBuilder<VendorsContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             using var context = new VendorsContext(options);
//             var repo = new Repository<Profile>(context);
//             var regionRepo = new Repository<Region>(context);
//             var service = new TestBlobService();
//             var id = CreateProfile(mediator, repo, regionRepo);

//             // When
//             var currentDirectory = Directory.GetCurrentDirectory();
//             var path = Path.Combine(currentDirectory, "Mock/Data/TestLogo4_TooLarge.jpg");
//             using var content = File.OpenRead(path);
//             var request = new AddProfileLogo("image/jpg", content.Length, content);
//             var handler = new AddProfileLogoHandler(mediator, repo, service);

//             //Then
//             await Assert.ThrowsAsync<InvalidContentLengthException>(async () => await handler.Handle(request, default));
//         }

//         private static async Task<long> CreateProfile(IMediator mediator, ICrudRepository<Profile> profileRepo, ICrudRepository<Region> regionRepo)
//         {
//             var profile = GetProfile();
//             var request = new CreateProfile(
//                 profile.Name, profile.PrimaryAddress, profile.AlternateAddress,
//                 profile.PhoneNumber, profile.FaxNumber, profile.WebAddress,
//                 profile.RegionId, profile.CommunicationMethod, profile.ProfileAccentColor);

//             var handler = new CreateProfileHandler(mediator, profileRepo, regionRepo);
//             return await handler.Handle(request, default);
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
