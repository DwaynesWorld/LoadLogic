using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class VendorMinorityStatusTest
    {
        [Fact]
        public void VendorMinorityStatus_ShouldCreate()
        {
            //Given
            var vendor = GetVendor();

            var certNumber = "1234567";
            var percent = (decimal)50.00;
            var type = new MinorityType("TEST", "Testing");


            //When
            var status = new VendorMinorityStatus(vendor, type, certNumber, percent);

            //Then
            Assert.NotNull(status);
        }

        [Fact]
        public void VendorMinorityStatus_ShouldUpdate()
        {
            //Given
            var vendor = GetVendor();

            var certNumber = "1234567";
            var percent = (decimal)50.00;
            var type = new MinorityType("TEST", "Testing");

            //When
            var status = new VendorMinorityStatus(vendor, type, certNumber, percent);
            var type2 = new MinorityType("TEST2", "Testing 2");
            status.Update(type2, "12345678", (decimal)90.00);

            //Then
            Assert.NotNull(status);
            Assert.Equal("12345678", status.CertificationNumber);
            Assert.Equal((decimal)90.00, status.Percent);
            Assert.Equal(type2, status.Type);
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
