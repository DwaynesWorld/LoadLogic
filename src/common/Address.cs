using System.Collections.Generic;

namespace LoadLogic.Services
{
    public class Address : ValueObject
    {
        public Address() { }

        public Address(
            string addressLine1,
            string addressLine2,
            string building,
            string city,
            string stateProvince,
            string countryRegion,
            string postalCode)
        {
            AddressLine1 = addressLine1 ?? "";
            AddressLine2 = addressLine2 ?? "";
            Building = building ?? "";
            City = city ?? "";
            StateProvince = stateProvince ?? "";
            CountryRegion = countryRegion ?? "";
            PostalCode = postalCode ?? "";
        }
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvince { get; set; } = string.Empty;
        public string CountryRegion { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;


        public static implicit operator string(Address address)
        {
            return address.ToString();
        }

        public override string ToString()
        {
            var fullAddress = "";
            if (!string.IsNullOrWhiteSpace(AddressLine1)) fullAddress += AddressLine1;
            if (!string.IsNullOrWhiteSpace(AddressLine2)) fullAddress += " " + AddressLine2;
            if (!string.IsNullOrWhiteSpace(Building)) fullAddress += " " + Building;
            if (!string.IsNullOrWhiteSpace(City)) fullAddress += ", " + City;
            if (!string.IsNullOrWhiteSpace(StateProvince)) fullAddress += " " + StateProvince;
            if (!string.IsNullOrWhiteSpace(CountryRegion)) fullAddress += ", " + CountryRegion;
            if (!string.IsNullOrWhiteSpace(PostalCode)) fullAddress += " " + PostalCode;
            return fullAddress;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return AddressLine1;
            yield return AddressLine2;
            yield return Building;
            yield return City;
            yield return StateProvince;
            yield return CountryRegion;
            yield return PostalCode;
        }
    }
}
