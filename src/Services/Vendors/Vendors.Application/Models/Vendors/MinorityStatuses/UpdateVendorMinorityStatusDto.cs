using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A data transfer object for requesting an update of an vendor minority status.
    /// </summary>
    public class UpdateVendorMinorityStatusDto
    {
        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; set; }

        /// <summary>
        /// The vendor minority status's unique identifier.
        /// </summary>
        [Required]
        public long StatusId { get; set; }

        /// <summary>
        /// The minority status type's unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; set; }

        /// <summary>
        /// The vendor's minority status certification number.
        /// </summary>
        public string CertificationNumber { get; set; } = string.Empty;

        /// <summary>
        /// The vendor's minority percentage.
        /// </summary>
        public decimal Percent { get; set; }
    }
}
