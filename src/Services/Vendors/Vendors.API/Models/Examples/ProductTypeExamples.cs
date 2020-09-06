using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace LoadLogic.Services.Vendors.API.Models.Examples
{
    public class ProductTypeDtoExample : IExamplesProvider<ProductTypeDto>
    {
        public ProductTypeDto GetExamples()
        {
            var example = new ProductTypeDto();
            example.Id = 1000;
            example.Code = "AGGM";
            example.Description = "Marine Aggregate";
            return example;
        }
    }

    public class EnumerableProductTypeDtoExample : IExamplesProvider<IEnumerable<ProductTypeDto>>
    {
        public IEnumerable<ProductTypeDto> GetExamples()
        {
            var examples = new List<ProductTypeDto>();

            var example1 = new ProductTypeDto();
            example1.Id = 1000;
            example1.Code = "AGGM";
            example1.Description = "Marine Aggregate";
            examples.Add(example1);

            var example2 = new ProductTypeDto();
            example2.Id = 1000;
            example2.Code = "SAND";
            example2.Description = "Sand";
            examples.Add(example2);

            return examples;
        }
    }

    public class CreateProductTypeDtoExample : IExamplesProvider<CreateProductTypeDto>
    {
        public CreateProductTypeDto GetExamples()
        {
            var example = new CreateProductTypeDto();
            example.Code = "AGGM";
            example.Description = "Marine Aggregate";
            return example;
        }
    }

    public class UpdateProductTypeDtoExample : IExamplesProvider<UpdateProductTypeDto>
    {
        public UpdateProductTypeDto GetExamples()
        {
            var example = new UpdateProductTypeDto();
            example.Code = "AGGM";
            example.Description = "Marine Aggregate";
            return example;
        }
    }
}
