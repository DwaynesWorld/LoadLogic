using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    /// <summary>
    /// A data transfer object for requesting an update of a profile's minority status.
    /// </summary>
    public class UpdateProfileMinorityStatusDto
    {
        /// <summary>
        /// The profile minority status's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The minority status's type unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; set; }

        /// <summary>
        /// The minority status certification number.
        /// </summary>
        public string CertificationNumber { get; set; } = string.Empty;

        /// <summary>
        /// The minority percentage.
        /// </summary>
        public decimal Percent { get; set; }
    }
}
