using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    public class ProfileDto
    {
        /// <summary>
        /// The Profile's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public Address? PrimaryAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Address? AlternateAddress { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string FaxNumber { get; set; } = string.Empty;

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

        /// <summary>
        /// 
        /// </summary>
        public List<MinorityStatusDto> MinorityStatuses { get; set; } = new List<MinorityStatusDto>();

        /// <summary>
        /// 
        /// </summary>
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        /// <summary>
        /// 
        /// </summary>
        public List<ContactDto> Vendors { get; set; } = new List<ContactDto>();


    }
}
