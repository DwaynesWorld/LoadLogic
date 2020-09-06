using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A data transfer object for requesting the update of an vendor products.
    /// </summary>
    public class UpdateVendorProductDto
    {
        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; set; }

        /// <summary>
        /// The vendor product's unique identifier.
        /// </summary>
        [Required]
        public long ProductId { get; set; }

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
