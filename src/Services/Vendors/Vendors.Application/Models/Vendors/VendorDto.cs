using LoadLogic.Services.Vendors.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A vendor data transfer object.
    /// </summary>
    public class VendorDto
    {

        /// <summary>
        /// The company's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The company's unique code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The company's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The company's type.
        /// </summary>
        public CompanyTypeDto? Type { get; set; }

        /// <summary>
        /// The company's primary address.
        /// </summary>
        public Address? PrimaryAddress { get; set; }

        /// <summary>
        /// The company's alternate address.
        /// </summary>
        public Address? AlternateAddress { get; set; }

        /// <summary>
        /// The vendor's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's fax number.
        /// </summary>
        public string FaxNumber { get; set; } = string.Empty;

        /// <summary>
        /// The company's website address.
        /// </summary>
        public string WebAddress { get; set; } = string.Empty;

        /// <summary>
        /// The region the company operates in.
        /// </summary>
        public RegionDto? Region { get; set; }

        /// <summary>
        /// The best form of communication of contacting this company.
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; set; }

        /// <summary>
        /// A flag indicating if this company is bonded.
        /// </summary>
        public bool IsBonded { get; set; }

        /// <summary>
        /// The rate at which this company is bonded.
        /// </summary>
        public decimal BondRate { get; set; }

        /// <summary>
        /// Notes about this company.
        /// </summary>
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// The list of statuses this company has.
        /// </summary>
        [Required]
        public List<MinorityStatusDto> MinorityStatuses { get; set; } = new List<MinorityStatusDto>();

        /// <summary>
        /// The list of products this company offers.
        /// </summary>
        [Required]
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        /// <summary>
        /// The list of contacts at this company.
        /// </summary>
        [Required]
        public List<ContactDto> Contacts { get; set; } = new List<ContactDto>();
    }
}
