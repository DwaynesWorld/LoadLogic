using System;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;

namespace LoadLogic.Services.Vendors.Application.Models
{
    public class MinorityStatusDto
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public MinorityTypeDto? Type { get; set; }
        public string CertificationNumber { get; set; } = string.Empty;
        public decimal Percent { get; set; }
    }
}
