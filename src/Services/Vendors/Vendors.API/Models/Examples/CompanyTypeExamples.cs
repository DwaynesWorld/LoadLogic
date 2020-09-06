using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Domain;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class CompanyTypeDtoExample : IExamplesProvider<CompanyTypeDto>
    {
        public CompanyTypeDto GetExamples()
        {
            var example = new CompanyTypeDto();
            example.Id = 1000;
            example.Code = "SUP";
            example.Description = "Supplier";
            return example;
        }
    }

    public class EnumerableCompanyTypeDtoExample : IExamplesProvider<IEnumerable<CompanyTypeDto>>
    {
        public IEnumerable<CompanyTypeDto> GetExamples()
        {
            var examples = new List<CompanyTypeDto>();

            var example1 = new CompanyTypeDto();
            example1.Id = 1000;
            example1.Code = "SUP";
            example1.Description = "Supplier";
            examples.Add(example1);

            var example2 = new CompanyTypeDto();
            example2.Id = 1000;
            example2.Code = "OWNER";
            example2.Description = "Owner";
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateCompanyTypeDtoExample : IExamplesProvider<CreateCompanyTypeDto>
    {
        public CreateCompanyTypeDto GetExamples()
        {
            var example = new CreateCompanyTypeDto();
            example.Code = "SUP";
            example.Description = "Supplier";
            return example;
        }
    }

    public class UpdateCompanyTypeDtoExample : IExamplesProvider<UpdateCompanyTypeDto>
    {
        public UpdateCompanyTypeDto GetExamples()
        {
            var example = new UpdateCompanyTypeDto();
            example.Id = 1000;
            example.Code = "MSUP";
            example.Description = "Material Supplier";
            return example;
        }
    }
}
