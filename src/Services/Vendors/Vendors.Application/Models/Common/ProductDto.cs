using System;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;

namespace LoadLogic.Services.Vendors.Application.Models
{
    public class ProductDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public ProductTypeDto? Product { get; set; }
        public RegionDto? Region { get; set; }
    }
}
