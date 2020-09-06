using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Commands.Profiles
{
    /// <summary>
    /// A data transfer object for requesting the creation of company products.
    /// </summary>
    public class CreateProfileProductDto
    {
        /// <summary>
        /// The product type's unique identifier.
        /// </summary>
        [Required]
        public long ProductTypeId { get; set; }

        /// <summary>
        /// The product region's unique identifier.
        /// </summary>
        public long? RegionId { get; set; }
    }
}
