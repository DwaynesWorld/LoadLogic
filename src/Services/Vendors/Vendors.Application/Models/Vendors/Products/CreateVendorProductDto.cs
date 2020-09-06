using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.Vendors
{
    /// <summary>
    /// A data transfer object for requesting the creation of vendor products.
    /// </summary>
    public class CreateVendorProductDto
    {
        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; set; }

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
