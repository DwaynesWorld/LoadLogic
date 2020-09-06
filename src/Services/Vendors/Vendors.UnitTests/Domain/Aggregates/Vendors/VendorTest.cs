using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class VendorTest
    {
        [Fact]
        public void ShouldCreate_WithRegion()
        {
            var faker = new Faker("en");

            //Given
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


            var region = new Region("TEST", "Testing");

            // When
            var vendor = new Vendor(
                code, name, null,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, region, communicationMethod,
                isBonded, bondRate, note);

            //Then
            Assert.NotNull(vendor);
        }

        [Fact]
        public void ShouldCreate_WithoutRegion()
        {
            var faker = new Faker("en");

            //Given
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

            // When
            var vendor = new Vendor(
                code, name, null,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod,
                isBonded, bondRate, note);

            //Then
            Assert.NotNull(vendor);
        }

        [Fact]
        public void ShouldThrow_WhenCodeIsNullEmptyOrWhiteSpace()
        {
            var faker = new Faker("en");

            //Given
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

            // When - Then
            Assert.Throws<ArgumentException>(() =>
                new Vendor(
                    string.Empty, name, null,
                    primaryAddress, alternateAddress,
                    phoneNumber, faxNumber,
                    webAddress, null, communicationMethod,
                    isBonded, bondRate, note));
        }

        [Fact]
        public void Should_SetMainContact_DuringAdditionOfContact()
        {
            var faker = new Faker("en");

            //Given
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

            var vendor = new Vendor(
                code, name, null,
                primaryAddress, alternateAddress,
                phoneNumber, faxNumber,
                webAddress, null, communicationMethod,
                isBonded, bondRate, note);

            var contactId1 = 999;
            var contactId2 = 9999;

            // When
            var contact1 = GetContact(contactId1, vendor);
            contact1.SetIsMainContact(true);
            vendor.AddContact(contact1);

            var contact2 = GetContact(contactId2, vendor);
            contact2.SetIsMainContact(true);
            vendor.AddContact(contact2);

            //Then
            Assert.False(vendor.FindContactById(contactId1).IsMainContact);
            Assert.True(vendor.FindContactById(contactId2).IsMainContact);
        }

        private static VendorContact GetContact(long id, Vendor vendor)
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

            return new VendorContact(
                id, vendor, fName, lName, title,
                phoneNumber, faxNumber, cellNumber,
                email, note, isMainContact);
        }
    }
}
