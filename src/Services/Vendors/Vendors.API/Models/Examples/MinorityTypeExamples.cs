using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class MinorityTypeDtoExample : IExamplesProvider<MinorityTypeDto>
    {
        public MinorityTypeDto GetExamples()
        {
            var example = new MinorityTypeDto();
            example.Id = 1000;
            example.Code = "DBE";
            example.Description = "Disadvantaged Business Enterprise";
            return example;
        }
    }

    public class EnumerableMinorityTypeDtoExample : IExamplesProvider<IEnumerable<MinorityTypeDto>>
    {
        public IEnumerable<MinorityTypeDto> GetExamples()
        {
            var examples = new List<MinorityTypeDto>();

            var example1 = new MinorityTypeDto();
            example1.Id = 1000;
            example1.Code = "DBE";
            example1.Description = "Disadvantaged Business Enterprise";
            examples.Add(example1);

            var example2 = new MinorityTypeDto();
            example2.Id = 1000;
            example2.Code = "MBE";
            example2.Description = "Minority Business Enterprise";
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateMinorityTypeDtoExample : IExamplesProvider<CreateMinorityTypeDto>
    {
        public CreateMinorityTypeDto GetExamples()
        {
            var example = new CreateMinorityTypeDto();
            example.Code = "DBE";
            example.Description = "Disadvantaged Business Enterprise";
            return example;
        }
    }

    public class UpdateMinorityTypeDtoExample : IExamplesProvider<UpdateMinorityTypeDto>
    {
        public UpdateMinorityTypeDto GetExamples()
        {
            var example = new UpdateMinorityTypeDto();
            example.Id = 1000;
            example.Code = "DBE";
            example.Description = "Disadvantaged Business Enterprise";
            return example;
        }
    }
}
