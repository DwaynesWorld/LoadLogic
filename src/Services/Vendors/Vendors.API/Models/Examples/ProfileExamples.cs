using Bogus;
using LoadLogic.Services.Vendors.Application.Commands.Profiles;
using LoadLogic.Services.Vendors.Application.Models.Profiles;
using LoadLogic.Services.Vendors.Domain;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class ProfileDtoExample : IExamplesProvider<ProfileDto>
    {
        public ProfileDto GetExamples()
        {
            var faker = new Faker("en");

            var example = new ProfileDto();
            example.Id = 1000;
            example.Name = faker.Company.CompanyName();
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

            example.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example.FaxNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example.WebAddress = faker.Internet.Url();
            example.Region = new RegionDtoExample().GetExamples();
            example.CommunicationMethod = CommunicationMethod.Email;
            example.ProfileAccentColor = "#009639";
            example.ProfileLogoUrl = faker.Image.LoremFlickrUrl();
            example.MinorityStatuses.AddRange(new EnumerableMinorityStatusDtoExample().GetExamples());
            example.Products.AddRange(new EnumerableProductDtoExample().GetExamples());
            example.Contacts.AddRange(new EnumerableContactDtoExample().GetExamples());
            return example;
        }
    }

    public class EnumerableProfileSummaryDtoExample : IExamplesProvider<IEnumerable<ProfileSummaryDto>>
    {
        public IEnumerable<ProfileSummaryDto> GetExamples()
        {
            var faker = new Faker("en");
            var examples = new List<ProfileSummaryDto>();

            var example1 = new ProfileSummaryDto();
            example1.Id = 1000;
            example1.Name = faker.Company.CompanyName();
            example1.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example1.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example1.WebAddress = faker.Internet.Url();
            example1.Region = new RegionDtoExample().GetExamples();
            example1.CommunicationMethod = CommunicationMethod.Email;
            example1.ProfileLogoUrl = faker.Image.LoremFlickrUrl();
            example1.ProfileAccentColor = "#009639";
            examples.Add(example1);

            var example2 = new ProfileSummaryDto();
            example2.Id = 1001;
            example2.Name = faker.Company.CompanyName();
            example2.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            example2.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            example2.WebAddress = faker.Internet.Url();
            example2.Region = new RegionDtoExample().GetExamples();
            example2.CommunicationMethod = CommunicationMethod.Email;
            example2.ProfileLogoUrl = faker.Image.LoremFlickrUrl();
            example2.ProfileAccentColor = "#009639";
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateProfileExample : IExamplesProvider<CreateProfileDto>
    {
        public CreateProfileDto GetExamples()
        {
            var faker = new Faker("en");

            var profile = new CreateProfileDto();

            profile.Name = faker.Company.CompanyName();
            profile.PrimaryAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            profile.AlternateAddress = new Address(
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                "123A",
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.Country(),
                "20066-7418");
            profile.PhoneNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            profile.FaxNumber = new PhoneNumber(faker.Phone.PhoneNumber("#########"));
            profile.WebAddress = faker.Internet.Url();
            profile.RegionId = null;
            profile.CommunicationMethod = CommunicationMethod.Email;
            profile.ProfileAccentColor = "#009639";

            return profile;
        }
    }

    public class UpdateProfileExample : IExamplesProvider<UpdateProfileDto>
    {
        public UpdateProfileDto GetExamples()
        {
            var faker = new Faker("en");

            var update = new UpdateProfileDto();

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
            update.RegionId = null;
            update.CommunicationMethod = CommunicationMethod.Email;
            update.ProfileAccentColor = "#009639";

            return update;
        }
    }

    public class CreateProfileContactDtoExample : IExamplesProvider<CreateProfileContactDto>
    {
        public CreateProfileContactDto GetExamples()
        {
            var c = new ContactDtoExample().GetExamples();
            var example = new CreateProfileContactDto();
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

    public class UpdateProfileContactExample : IExamplesProvider<UpdateProfileContactDto>
    {
        public UpdateProfileContactDto GetExamples()
        {
            var c = new ContactDtoExample().GetExamples();
            var example = new UpdateProfileContactDto();
            example.Id = c.Id;
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

    public class CreateProfileMinorityStatusDtoExample : IExamplesProvider<CreateProfileMinorityStatusDto>
    {
        public CreateProfileMinorityStatusDto GetExamples()
        {
            var example = new CreateProfileMinorityStatusDto();
            example.MinorityTypeId = 1000;
            example.CertificationNumber = "12345-678";
            example.Percent = 49.9m;
            return example;
        }
    }

    public class UpdateProfileMinorityStatusDtoExample : IExamplesProvider<UpdateProfileMinorityStatusDto>
    {
        public UpdateProfileMinorityStatusDto GetExamples()
        {
            var example = new UpdateProfileMinorityStatusDto();
            example.Id = 1000;
            example.MinorityTypeId = 1000;
            example.CertificationNumber = "12345-678";
            example.Percent = 49.9m;
            return example;
        }
    }

    public class CreateProfileProductDtoExample : IExamplesProvider<CreateProfileProductDto>
    {
        public CreateProfileProductDto GetExamples()
        {
            var example = new CreateProfileProductDto();
            example.ProductTypeId = 1000;
            example.RegionId = 1000;
            return example;
        }
    }

    public class UpdateProfileProductDtoExample : IExamplesProvider<UpdateProfileProductDto>
    {
        public UpdateProfileProductDto GetExamples()
        {
            var example = new UpdateProfileProductDto();
            example.ProductTypeId = 1000;
            example.RegionId = 1000;
            return example;
        }
    }
}
