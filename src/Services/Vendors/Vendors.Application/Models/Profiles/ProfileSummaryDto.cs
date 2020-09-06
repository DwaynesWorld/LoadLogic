using System;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    public class ProfileSummaryDto
    {
        /// <summary>
        /// The Profile's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public Address? PrimaryAddress { get; set; }

        public PhoneNumber? PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebAddress { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public RegionDto? Region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; set; }

        /// <summary>
        /// The accent color for the company.
        /// </summary>
        public string ProfileAccentColor { get; set; } = string.Empty;

        /// <summary>
        /// The public logo url for the company.
        /// </summary>
        public string ProfileLogoUrl { get; set; } = string.Empty;
    }
}
