using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A vendor summary data transfer object.
    /// </summary>
    public class VendorSummaryDto
    {

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The vendors's unique code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's name.
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
        /// The vendor's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

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
    }
}
