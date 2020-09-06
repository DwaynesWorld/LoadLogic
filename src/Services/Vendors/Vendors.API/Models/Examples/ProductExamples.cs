using System;
using System.Collections.Generic;
using LoadLogic.Services.Vendors.Application.Models;
using Swashbuckle.AspNetCore.Filters;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class ProductDtoExample : IExamplesProvider<ProductDto>
    {
        public ProductDto GetExamples()
        {
            var companyId = 1000;

            var example = new ProductDto();
            example.Id = 1000;
            example.CompanyId = companyId;
            example.Product = new ProductTypeDtoExample().GetExamples();
            example.Region = new RegionDtoExample().GetExamples();
            return example;
        }
    }

    public class EnumerableProductDtoExample : IExamplesProvider<IEnumerable<ProductDto>>
    {
        public IEnumerable<ProductDto> GetExamples()
        {
            var companyId = 1000;

            var examples = new List<ProductDto>();

            var example1 = new ProductDto();
            example1.Id = 1000;
            example1.CompanyId = companyId;
            example1.Product = new ProductTypeDtoExample().GetExamples();
            example1.Region = new RegionDtoExample().GetExamples();
            examples.Add(example1);

            var example2 = new ProductDto();
            example2.Id = 1000;
            example2.CompanyId = companyId;
            example2.Product = new ProductTypeDtoExample().GetExamples();
            example2.Region = new RegionDtoExample().GetExamples();
            examples.Add(example2);

            return examples;
        }
    }
}
