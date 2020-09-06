using LoadLogic.Services.Vendors.Application.Commands.Profiles;
using LoadLogic.Services.Vendors.Application.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class MinorityStatusDtoExample : IExamplesProvider<MinorityStatusDto>
    {
        public MinorityStatusDto GetExamples()
        {
            var example = new MinorityStatusDto();
            example.Id = 1000;
            example.CompanyId = 1000;
            example.Type = new MinorityTypeDtoExample().GetExamples();
            example.CertificationNumber = "111000";
            example.Percent = (decimal)50.00;
            return example;
        }
    }

    public class EnumerableMinorityStatusDtoExample : IExamplesProvider<IEnumerable<MinorityStatusDto>>
    {
        public IEnumerable<MinorityStatusDto> GetExamples()
        {
            var examples = new List<MinorityStatusDto>();

            var companyId = 1000;

            var example1 = new MinorityStatusDto();
            example1.Id = 1000;
            example1.CompanyId = companyId;
            example1.Type = new MinorityTypeDtoExample().GetExamples();
            example1.CertificationNumber = "111-000";
            example1.Percent = 50.1m;
            examples.Add(example1);

            var example2 = new MinorityStatusDto();
            example2.Id = 1000;
            example2.CompanyId = companyId;
            example2.Type = new MinorityTypeDtoExample().GetExamples();
            example2.CertificationNumber = "111-001";
            example2.Percent = 97.2m;
            examples.Add(example2);

            return examples;
        }
    }
}
