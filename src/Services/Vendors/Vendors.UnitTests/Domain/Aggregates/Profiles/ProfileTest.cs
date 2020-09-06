using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class ProfileTest
    {
        [Fact]
        public void ShouldCreate_WithRegion()
        {
            var faker = new Faker("en");

            //Given
            var name = faker.Company.CompanyName();
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#EEE";


            var region = new Region("TEST", "Testing");

            // When
            var profile = new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, region, communicationMethod, profileAccentColor);

            //Then
            Assert.NotNull(profile);
        }

        [Fact]
        public void ShouldCreate_WithoutRegion()
        {
            var faker = new Faker("en");

            //Given
            var name = faker.Company.CompanyName();
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#EEE";

            // When
            var profile = new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor);

            //Then
            Assert.NotNull(profile);
        }

        [Fact]
        public void ShouldThrow_WhenCompanyNameIsNullEmptyOrWhiteSpace()
        {
            var faker = new Faker("en");

            //Given
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#EEE";

            // When - Then
            Assert.Throws<ArgumentException>(() => new Profile(
                string.Empty,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor));
        }


        [Fact]
        public void Should_SetMainContact_DuringAdditionOfContact()
        {
            var faker = new Faker("en");

            //Given
            var name = faker.Company.CompanyName();
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#EEE";

            var profile = new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor);

            var contactId1 = 9999;
            var contactId2 = 9998;

            // When
            var contact1 = GetContact(contactId1, profile);
            contact1.SetIsMainContact(true);
            profile.AddContact(contact1);

            var contact2 = GetContact(contactId2, profile);
            contact2.SetIsMainContact(true);
            profile.AddContact(contact2);

            //Then
            Assert.False(profile.FindContactById(contactId1).IsMainContact);
            Assert.True(profile.FindContactById(contactId2).IsMainContact);
        }

        [Fact]
        public void ShouldThrow_WhenProfileIsCreatedWithInvalidAccentColor()
        {
            var faker = new Faker("en");

            //Given
            var name = faker.Company.CompanyName();
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#ggg";

            Assert.Throws<InvalidHexColorException>(() => new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor));
        }

        [Fact]
        public void ShouldThrow_WhenProfileIsUpdatedWithInvalidAccentColor()
        {
            var faker = new Faker("en");

            //Given
            var name = faker.Company.CompanyName();
            var primaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var alternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var webAddress = faker.Internet.Url();
            var communicationMethod = CommunicationMethod.Email;
            var profileAccentColor = "#eee";
            var profile = new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor);

            Assert.Throws<InvalidHexColorException>(() => profile.Update(
                name, primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, "#zzz"));
        }

        private static ProfileContact GetContact(long id, Profile profile)
        {
            var fName = "John";
            var lName = "Doe";
            var title = "Sr. CEO";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = false;

            return new ProfileContact(
                id, profile, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);
        }
    }
}
