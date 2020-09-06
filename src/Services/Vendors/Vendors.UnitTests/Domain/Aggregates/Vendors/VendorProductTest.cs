using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class VendorProductTest
    {
        [Fact]
        public void VendorProduct_ShouldCreate_WithRegion()
        {
            //Given
            var vendor = GetVendor();

            var type = new ProductType("TEST", "Testing");
            var region = new Region("TEST", "Testing");

            //When
            var product = new VendorProduct(vendor, type, region);

            //Then
            Assert.NotNull(product);
        }

        [Fact]
        public void VendorProduct_ShouldCreate_WithoutRegion()
        {
            //Given
            var vendor = GetVendor();

            var type = new ProductType("TEST", "Testing");

            //When
            var product = new VendorProduct(vendor, type, null);

            //Then
            Assert.NotNull(product);
        }

        [Fact]
        public void VendorProduct_ShouldUpdate_WithRegion()
        {
            //Given
            var vendor = GetVendor();

            var type = new ProductType("TEST", "Testing");
            var region = new Region("TEST", "Testing");
            var product = new VendorProduct(vendor, type, region);

            var type2Id = 999;
            var region2Id = 999;

            //When
            var type2 = new ProductType(type2Id, "TEST2", "Testing 2");
            var region2 = new Region(region2Id, "TEST2", "Testing 2");
            product.Update(type2, region2);

            //Then
            Assert.NotNull(product);
            Assert.Equal(type2, product.Type);
            Assert.Equal(region2, product.Region);
        }

        [Fact]
        public void VendorProduct_ShouldUpdate_WithoutRegion()
        {
            //Given
            var vendor = GetVendor();

            var type = new ProductType("TEST", "Testing");
            var product = new VendorProduct(vendor, type, null);

            var type2Id = 999;
            var region2Id = 999;

            //When
            var type2 = new ProductType(type2Id, "TEST2", "Testing 2");
            var region2 = new Region(region2Id, "TEST2", "Testing 2");
            product.Update(type2, region2);

            //Then
            Assert.NotNull(product);
            Assert.Equal(type2, product.Type);
            Assert.Equal(region2, product.Region);
        }

        [Fact]
        public void VendorProduct_ShouldUpdate_WithoutRegion2()
        {
            //Given
            var vendor = GetVendor();

            var type = new ProductType("TEST", "Testing");
            var product = new VendorProduct(vendor, type, null);

            var type2Id = 999;

            //When
            var type2 = new ProductType(type2Id, "TEST2", "Testing 2");
            product.Update(type2, null);

            //Then
            Assert.NotNull(product);
            Assert.Equal(type2, product.Type);
            Assert.Null(product.Region);
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
