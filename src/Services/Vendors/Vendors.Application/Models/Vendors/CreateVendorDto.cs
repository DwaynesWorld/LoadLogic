using System;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A data transfer object for requesting the creation on a vendor.
    /// </summary>
    public class CreateVendorDto
    {
        /// <summary>
        /// The vendor's unique code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's type unique identifier.
        /// </summary>
        public long? TypeId { get; set; }

        /// <summary>
        /// The vendor's primary address.
        /// </summary>
        public Address? PrimaryAddress { get; set; }

        /// <summary>
        /// The vendor's alternate address.
        /// </summary>
        public Address? AlternateAddress { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string FaxNumber { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's website address.
        /// </summary>
        public string WebAddress { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the region the vendor operates in.
        /// </summary>
        public long? RegionId { get; set; }

        /// <summary>
        /// The best form of communication of contacting this vendor.
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; set; }

        /// <summary>
        /// A flag indicating if this vendor is bonded.
        /// </summary>
        public bool IsBonded { get; set; }

        /// <summary>
        /// The rate at which this vendor is bonded.
        /// </summary>
        public decimal BondRate { get; set; }

        /// <summary>
        /// Notes about this vendor.
        /// </summary>
        public string Note { get; set; } = string.Empty;
    }
}
