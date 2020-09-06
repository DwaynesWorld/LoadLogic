using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class ProfileContactTest
    {
        [Fact]
        public void ProfileContact_ShouldCreate()
        {
            //Given
            var profile = GetProfile();

            var fName = "John";
            var lName = "Dow";
            var title = "Sr. Manager";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = true;

            //When
            var contact = new ProfileContact(
                profile, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);

            //Then
            Assert.NotNull(contact);
        }

        [Fact]
        public void ProfileContact_ShouldUpdate()
        {
            //Given
            var profile = GetProfile();

            var fName = "John";
            var lName = "Dow";
            var title = "Sr. Manager";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = true;
            var contact = new ProfileContact(
                profile, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);

            //When
            contact.Update(
                fName, "Doe", title,
                phoneNumber, faxNumber, cellNumber,
                email, note, false);

            //Then
            Assert.Equal("Doe", contact.LastName);
            Assert.False(contact.IsMainContact);
        }

        [Fact]
        public void ProfileContact_ShouldSetMainContact()
        {
            //Given
            var profile = GetProfile();

            var fName = "John";
            var lName = "Doe";
            var title = "Sr. Manager";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = true;
            var contact = new ProfileContact(
                profile, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);

            //When
            contact.SetIsMainContact(false);

            //Then
            Assert.False(contact.IsMainContact);
        }

        private static Profile GetProfile()
        {
            var faker = new Faker("en");
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

            return new Profile(
                name,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod, profileAccentColor);
        }
    }
}
