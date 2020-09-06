
using Bogus;
using LoadLogic.Services;

namespace LoadLogic.Services.Vendors.UnitTests
{
    public static class Mock
    {
        public static Address GetAddress()
        {
            var faker = new Faker("en");

            return new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.BuildingNumber(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                faker.Address.ZipCode());
        }

        public static PhoneNumber GetPhoneNumber()
        {
            var faker = new Faker("en");
            return new PhoneNumber(faker.Phone.PhoneNumber("##########"));
        }
    }
}
