using Bogus;
using LoadLogic.Services.Vendors.Application.Commands.Vendors;
using LoadLogic.Services.Vendors.Application.Models.Vendors;
using LoadLogic.Services.Vendors.Domain;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class VendorDtoExample : IExamplesProvider<VendorDto>
    {
        public VendorDto GetExamples()
        {
            var faker = new Faker("en");

            var example = new VendorDto();
            example.Id = 1000;
            example.Code = faker.Random.String2(2, 4).ToUpper();
            example.IsBonded = faker.Random.Bool();
            example.BondRate = 37.5m;
            example.Note = faker.Lorem.Sentences(3);
            example.Name = faker.Company.CompanyName();
            example.WebAddress = faker.Internet.Url();
            example.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example.FaxNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example.CommunicationMethod = CommunicationMethod.Email;
            example.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example.AlternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example.Type = new CompanyTypeDtoExample().GetExamples();
            example.Region = new RegionDtoExample().GetExamples();
            example.MinorityStatuses.AddRange(new EnumerableMinorityStatusDtoExample().GetExamples());
            example.Products.AddRange(new EnumerableProductDtoExample().GetExamples());
            example.Vendors.AddRange(new EnumerableContactDtoExample().GetExamples());
            return example;
        }
    }

    public class EnumerableVendorSummaryDtoExample : IExamplesProvider<IEnumerable<VendorSummaryDto>>
    {
        public IEnumerable<VendorSummaryDto> GetExamples()
        {
            var faker = new Faker("en");
            var examples = new List<VendorSummaryDto>();

            var example1 = new VendorSummaryDto();
            example1.Id = 1000;
            example1.Code = faker.Random.String2(2, 4).ToUpper();
            example1.IsBonded = faker.Random.Bool();
            example1.Type = new CompanyTypeDtoExample().GetExamples();
            example1.BondRate = 37.5m;
            example1.Note = faker.Lorem.Sentences(3);
            example1.Name = faker.Company.CompanyName();
            example1.WebAddress = faker.Internet.Url();
            example1.CommunicationMethod = CommunicationMethod.Email;
            example1.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example1.Region = new RegionDtoExample().GetExamples();
            examples.Add(example1);

            var example2 = new VendorSummaryDto();
            example2.Id = 1000;
            example2.Code = faker.Random.String2(2, 4).ToUpper();
            example2.IsBonded = faker.Random.Bool();
            example2.Type = new CompanyTypeDtoExample().GetExamples();
            example2.BondRate = 37.5m;
            example2.Note = faker.Lorem.Sentences(3);
            example2.Name = faker.Company.CompanyName();
            example2.WebAddress = faker.Internet.Url();
            example2.CommunicationMethod = CommunicationMethod.Email;
            example2.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example2.Region = new RegionDtoExample().GetExamples();
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateVendorExample : IExamplesProvider<CreateVendorDto>
    {
        public CreateVendorDto GetExamples()
        {
            var faker = new Faker("en");

            var create = new CreateVendorDto();

            create.Code = faker.Random.String2(2, 4).ToUpper();
            create.Name = faker.Company.CompanyName();
            create.TypeId = 1000;
            create.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            create.AlternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            create.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            create.FaxNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            create.WebAddress = faker.Internet.Url();
            create.RegionId = 1000;
            create.CommunicationMethod = CommunicationMethod.Email;
            create.IsBonded = faker.Random.Bool();
            create.BondRate = 37.5m;
            create.Note = faker.Lorem.Sentences(3);

            return create;
        }
    }

    public class UpdateVendorExample : IExamplesProvider<UpdateVendorDto>
    {
        public UpdateVendorDto GetExamples()
        {
            var faker = new Faker("en");

            var update = new UpdateVendorDto();

            update.Code = faker.Random.String2(2, 4).ToUpper();
            update.Name = faker.Company.CompanyName();
            update.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            update.AlternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            update.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            update.FaxNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            update.WebAddress = faker.Internet.Url();
            update.RegionId = 1000;
            update.CommunicationMethod = CommunicationMethod.Email;
            update.IsBonded = faker.Random.Bool();
            update.BondRate = 37.5m;
            update.Note = faker.Lorem.Sentences(3);

            return update;
        }
    }

    public class CreateVendorContactDtoExample : IExamplesProvider<CreateVendorContactDto>
    {
        public CreateVendorContactDto GetExamples()
        {
            var c = new ContactDtoExample().GetExamples();

            var example = new CreateVendorContactDto();
            example.VendorId = 1000;
            example.FirstName = c.FirstName;
            example.LastName = c.LastName;
            example.Title = c.Title;
            example.PhoneNumber = c.PhoneNumber;
            example.FaxNumber = c.FaxNumber;
            example.CellPhoneNumber = c.CellPhoneNumber;
            example.EmailAddress = c.EmailAddress;
            example.Note = c.Note;
            example.IsMainContact = c.IsMainContact;
            return example;
        }
    }

    public class UpdateVendorContactDtoExample : IExamplesProvider<UpdateVendorContactDto>
    {
        public UpdateVendorContactDto GetExamples()
        {
            var c = new ContactDtoExample().GetExamples();

            var example = new UpdateVendorContactDto();
            example.VendorId = 1000;
            example.ContactId = c.Id;
            example.FirstName = c.FirstName;
            example.LastName = c.LastName;
            example.Title = c.Title;
            example.PhoneNumber = c.PhoneNumber;
            example.FaxNumber = c.FaxNumber;
            example.CellPhoneNumber = c.CellPhoneNumber;
            example.EmailAddress = c.EmailAddress;
            example.Note = c.Note;
            example.IsMainContact = c.IsMainContact;
            return example;
        }
    }

    public class CreateVendorMinorityStatusDtoExample : IExamplesProvider<CreateVendorMinorityStatusDto>
    {
        public CreateVendorMinorityStatusDto GetExamples()
        {
            var example = new CreateVendorMinorityStatusDto();
            example.VendorId = 1000;
            example.MinorityTypeId = 1000;
            example.CertificationNumber = "12345-678";
            example.Percent = 49.9m;
            return example;
        }
    }

    public class UpdateVendorMinorityStatusDtoExample : IExamplesProvider<UpdateVendorMinorityStatusDto>
    {
        public UpdateVendorMinorityStatusDto GetExamples()
        {
            var example = new UpdateVendorMinorityStatusDto();
            example.VendorId = 1000;
            example.StatusId = 1000;
            example.MinorityTypeId = 1000;
            example.CertificationNumber = "12345-678";
            example.Percent = 49.9m;
            return example;
        }
    }

    public class CreateVendorProductDtoExample : IExamplesProvider<CreateVendorProductDto>
    {
        public CreateVendorProductDto GetExamples()
        {
            var example = new CreateVendorProductDto();
            example.VendorId = 1000;
            example.ProductTypeId = 1000;
            example.RegionId = 1000;
            return example;
        }
    }

    public class UpdateVendorProductDtoExample : IExamplesProvider<UpdateVendorProductDto>
    {
        public UpdateVendorProductDto GetExamples()
        {
            var example = new UpdateVendorProductDto();
            example.VendorId = 1000;
            example.ProductId = 1000;
            example.ProductTypeId = 1000;
            example.RegionId = 1000;
            return example;
        }
    }
}
