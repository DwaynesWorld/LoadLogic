using LoadLogic.Services.Vendors.Application.Commands.Regions;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class RegionDtoExample : IExamplesProvider<RegionDto>
    {
        public RegionDto GetExamples()
        {
            var example = new RegionDto();
            example.Id = 1000;
            example.Code = "SE";
            example.Description = "Southeast Region";
            return example;
        }
    }

    public class EnumerableRegionDtoExample : IExamplesProvider<IEnumerable<RegionDto>>
    {
        public IEnumerable<RegionDto> GetExamples()
        {
            var examples = new List<RegionDto>();

            var example1 = new RegionDto();
            example1.Id = 1000;
            example1.Code = "SE";
            example1.Description = "Southeast Region";
            examples.Add(example1);

            var example2 = new RegionDto();
            example2.Id = 1000;
            example2.Code = "NW";
            example2.Description = "Northwest Region";
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateRegionDtoExample : IExamplesProvider<CreateRegionDto>
    {
        public CreateRegionDto GetExamples()
        {
            var example = new CreateRegionDto();
            example.Code = "SE";
            example.Description = "Southeast Region";
            return example;
        }
    }

    public class UpdateRegionDtoExample : IExamplesProvider<UpdateRegionDto>
    {
        public UpdateRegionDto GetExamples()
        {
            var example = new UpdateRegionDto();
            example.Code = "SE";
            example.Description = "Southeast Region";
            return example;
        }
    }
}
