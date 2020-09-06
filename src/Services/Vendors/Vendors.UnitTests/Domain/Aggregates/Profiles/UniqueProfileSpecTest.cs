// using System;
// using Bogus;
// using LoadLogic.Services;
// using LoadLogic.Services.Vendors.Domain;
// using Xunit;

// namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
// {
//     public class UniqueProfileSpecTest
//     {
//         [Fact]
//         public void UniqueProfileSpec_ShouldCreate()
//         {
//             //Given 

//             //When
//             var spec = new UniqueProfileSpec();

//             //Then 
//             Assert.NotNull(spec);
//         }

//         [Fact]
//         public void UniqueProfileSpec_ShouldCheckSatificationOfExpression()
//         {
//             //Given 
//             var profile = GetProfile();
//             var profile2 = GetProfile();

//             //When
//             var spec = new UniqueProfileSpec();

//             //Then 
//             Assert.NotNull(spec);
//             Assert.True(spec.IsSatisfiedBy(profile));
//             Assert.False(spec.IsSatisfiedBy(profile2));
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
//             var phoneNumber = (PhoneNumber)"1112223333";
//             var faxNumber = (PhoneNumber)"1112223333";
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
