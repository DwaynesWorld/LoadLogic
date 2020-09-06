using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Regions
{
    /// <summary>
    /// A Region data transfer object.
    /// </summary>
    public class RegionDto
    {
        /// <summary>
        /// The Region's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Region code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Region description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
