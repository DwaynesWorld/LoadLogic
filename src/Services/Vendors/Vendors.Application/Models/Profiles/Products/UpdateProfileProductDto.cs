using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Profiles
{
    /// <summary>
    /// An immutable command message for requesting the update of an company products.
    /// </summary>
    public class UpdateProfileProductDto
    {
        /// <summary>
        /// The company product's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The product types's unique identifier.
        /// </summary>
        [Required]
        public long ProductTypeId { get; set; }

        /// <summary>
        /// The product region's unique identifier.
        /// </summary>
        public long? RegionId { get; set; }
    }
}
