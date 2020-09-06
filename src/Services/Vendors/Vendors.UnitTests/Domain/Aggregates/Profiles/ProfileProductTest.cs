using System;
using Bogus;
using LoadLogic.Services;
using LoadLogic.Services.Vendors.Domain;
using Xunit;

namespace LoadLogic.Services.Vendors.UnitTests.Aggregates
{
    public class ProfileProductTest
    {
        [Fact]
        public void ProfileProduct_ShouldCreate_WithRegion()
        {
            //Given
            var profile = GetProfile();

            var type = new ProductType("TEST", "Testing");
            var region = new Region("TEST", "Testing");

            //When
            var product = new ProfileProduct(profile, type, region);

            //Then
            Assert.NotNull(product);
        }

        [Fact]
        public void ProfileProduct_ShouldCreate_WithoutRegion()
        {
            //Given
            var profile = GetProfile();
            var type = new ProductType("TEST", "Testing");

            //When
            var product = new ProfileProduct(profile, type, null);

            //Then
            Assert.NotNull(product);
        }

        [Fact]
        public void ProfileProduct_ShouldUpdate_WithRegion()
        {
            //Given
            var profile = GetProfile();

            var type = new ProductType("TEST", "Testing");
            var region = new Region("TEST", "Testing");
            var product = new ProfileProduct(profile, type, region);

            var type2Id = 9999;
            var region2Id = 9999;

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
        public void ProfileProduct_ShouldUpdate_WithoutRegion()
        {
            //Given
            var profile = GetProfile();

            var type = new ProductType("TEST", "Testing");
            var product = new ProfileProduct(profile, type, null);

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
        public void ProfileProduct_ShouldUpdate_WithoutRegion2()
        {
            //Given
            var profile = GetProfile();
            var type = new ProductType("TEST", "Testing");
            var product = new ProfileProduct(profile, type, null);

            var type2Id = 999;

            //When
            var type2 = new ProductType(type2Id, "TEST2", "Testing 2");
            product.Update(type2, null);

            //Then
            Assert.NotNull(product);
            Assert.Equal(type2, product.Type);
            Assert.Null(product.Region);
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
