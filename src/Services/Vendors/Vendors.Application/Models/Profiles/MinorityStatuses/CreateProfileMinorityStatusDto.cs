using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    /// <summary>
    /// A data transfer object for requesting the creation of an company minority status.
    /// </summary>
    public class CreateProfileMinorityStatusDto
    {
        /// <summary>
        /// The minority status type's unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; set; }

        /// <summary>
        /// The company's minority certification number.
        /// </summary>
        public string CertificationNumber { get; set; } = string.Empty;

        /// <summary>
        /// The company's minority percentage.
        /// </summary>
        public decimal Percent { get; set; }
    }
}
