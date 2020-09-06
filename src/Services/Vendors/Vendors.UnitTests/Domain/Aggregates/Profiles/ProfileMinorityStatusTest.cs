using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class ProfileMinorityStatusTest
    {
        [Fact]
        public void ProfileMinorityStatus_ShouldCreate()
        {
            //Given
            var profile = GetProfile();

            var certNumber = "1234567";
            var percent = (decimal)50.00;
            var type = new MinorityType("TEST", "Testing");

            //When
            var status = new ProfileMinorityStatus(profile, type, certNumber, percent);

            //Then
            Assert.NotNull(status);
        }

        [Fact]
        public void ProfileMinorityStatus_ShouldUpdate()
        {
            //Given
            var profile = GetProfile();

            var certNumber = "1234567";
            var percent = (decimal)50.00;
            var type = new MinorityType("TEST", "Testing");

            //When
            var status = new ProfileMinorityStatus(profile, type, certNumber, percent);
            var type2 = new MinorityType("TEST2", "Testing 2");
            status.Update(type2, "12345678", (decimal)90.00);

            //Then
            Assert.NotNull(status);
            Assert.Equal("12345678", status.CertificationNumber);
            Assert.Equal((decimal)90.00, status.Percent);
            Assert.Equal(type2, status.Type);
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
