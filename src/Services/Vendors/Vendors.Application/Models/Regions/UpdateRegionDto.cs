using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Regions
{
    /// <summary>
    /// A data transfer object for requesting the update of a Region.
    /// </summary>
    public class UpdateRegionDto
    {
        /// <summary>
        /// The existing region's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The the region's unique code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The region's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
