using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class UniqueVendorSpecTest
    {
        [Fact]
        public void UniqueVendorSpec_ShouldCreate()
        {
            //Given 
            var code = "TEST";

            //When
            var spec = new UniqueVendorSpec(code);

            //Then 
            Assert.NotNull(spec);
        }

        [Fact]
        public void UniqueVendorSpec_ShouldCheckSatificationOfExpression()
        {
            //Given 
            var vendor = GetVendor();
            var vendor2 = GetVendor();

            //When
            var spec = new UniqueVendorSpec(vendor.Code);

            //Then 
            Assert.NotNull(spec);
            Assert.True(spec.IsSatisfiedBy(vendor));
            Assert.False(spec.IsSatisfiedBy(vendor2));
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
