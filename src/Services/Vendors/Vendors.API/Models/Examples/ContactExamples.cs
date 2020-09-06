using Bogus;
using LoadLogic.Services.Vendors.Application.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class ContactDtoExample : IExamplesProvider<ContactDto>
    {
        public ContactDto GetExamples()
        {
            var faker = new Faker("en");
            var format = "###-###-####";

            var example = new ContactDto();
            example.Id = 1000;
            example.FirstName = faker.Person.FirstName;
            example.LastName = faker.Person.LastName;
            example.Title = faker.Name.JobTitle();
            example.PhoneNumber = faker.Phone.PhoneNumber(format);
            example.FaxNumber = faker.Phone.PhoneNumber(format);
            example.CellPhoneNumber = faker.Phone.PhoneNumber(format);
            example.EmailAddress = faker.Person.Email;
            example.IsMainContact = false;
            example.Note = faker.Lorem.Sentences(3);
            return example;
        }
    }

    public class EnumerableContactDtoExample : IExamplesProvider<IEnumerable<ContactDto>>
    {
        public IEnumerable<ContactDto> GetExamples()
        {
            var faker = new Faker("en");
            var format = "###-###-####";

            var examples = new List<ContactDto>();

            var example1 = new ContactDto();
            example1.Id = 1000;
            example1.FirstName = faker.Person.FirstName;
            example1.LastName = faker.Person.LastName;
            example1.Title = faker.Name.JobTitle();
            example1.PhoneNumber = faker.Phone.PhoneNumber(format);
            example1.FaxNumber = faker.Phone.PhoneNumber(format);
            example1.CellPhoneNumber = faker.Phone.PhoneNumber(format);
            example1.EmailAddress = faker.Person.Email;
            example1.IsMainContact = false;
            example1.Note = faker.Lorem.Sentences(3);
            examples.Add(example1);

            var example2 = new ContactDto();
            example2.Id = 1000;
            example2.FirstName = faker.Person.FirstName;
            example2.LastName = faker.Person.LastName;
            example2.Title = faker.Name.JobTitle();
            example2.PhoneNumber = faker.Phone.PhoneNumber(format);
            example2.FaxNumber = faker.Phone.PhoneNumber(format);
            example2.CellPhoneNumber = faker.Phone.PhoneNumber(format);
            example2.EmailAddress = faker.Person.Email;
            example2.IsMainContact = false;
            example2.Note = faker.Lorem.Sentences(3);
            examples.Add(example2);

            return examples;
        }
    }
}
