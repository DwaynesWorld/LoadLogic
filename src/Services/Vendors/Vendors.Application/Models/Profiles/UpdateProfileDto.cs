using System;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    /// <summary>
    /// A data transfer object for requesting the update to an existing company profile.
    /// </summary>
    public class UpdateProfileDto
    {
        /// <summary>
        /// The company's name.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The company's primary address.
        /// </summary>
        public Address? PrimaryAddress { get; set; }

        /// <summary>
        /// The company's alternate address.
        /// </summary>
        public Address? AlternateAddress { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string FaxNumber { get; set; } = string.Empty;

        /// <summary>
        /// The company's website address.
        /// </summary>
        public string WebAddress { get; set; } = string.Empty;

        /// <summary>
        /// The unique identifier of the region the company operates in.
        /// </summary>
        public long? RegionId { get; set; }

        /// <summary>
        /// The best form of communication of contacting this company.
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; set; }

        /// <summary>
        /// The accent color for the company.
        /// </summary>
        public string ProfileAccentColor { get; set; } = string.Empty;
    }
}
