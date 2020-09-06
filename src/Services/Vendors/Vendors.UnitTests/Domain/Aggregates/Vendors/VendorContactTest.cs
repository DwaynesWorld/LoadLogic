using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class VendorContactTest
    {
        [Fact]
        public void VendorContact_ShouldCreate()
        {
            //Given
            var vendor = GetVendor();

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
            var contact = new VendorContact(
                vendor, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);

            //Then
            Assert.NotNull(contact);
        }

        [Fact]
        public void VendorContact_ShouldUpdate()
        {
            //Given
            var vendor = GetVendor();

            var fName = "John";
            var lName = "Dow";
            var title = "Sr. Manager";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = true;
            var contact = new VendorContact(
                vendor, fName, lName, title,
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
        public void VendorContact_ShouldSetMainContact()
        {
            //Given
            var vendor = GetVendor();

            var fName = "John";
            var lName = "Doe";
            var title = "Sr. Manager";
            var phoneNumber = (PhoneNumber)"1112223333";
            var faxNumber = (PhoneNumber)"1112223333";
            var cellNumber = (PhoneNumber)"1112223333";
            var email = (Email)"john.dow@company.com";
            var note = string.Empty;
            var isMainContact = true;
            var contact = new VendorContact(
                vendor, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);

            //When
            contact.SetIsMainContact(false);

            //Then
            Assert.False(contact.IsMainContact);
        }

        private static Vendor GetVendor()
        {
            var faker = new Faker("en");

            var code = faker.Random.String2(2, 4).ToUpper();
            var isBonded = faker.Random.Bool();
            var bondRate = isBonded ? faker.Random.Decimal((decimal)0.5, (decimal)100.0) : (decimal)0.0;
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
            var note = faker.Lorem.Sentences(3);

            return new Vendor(
                code, name, null,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod,
                isBonded, bondRate, note);
        }
    }
}
