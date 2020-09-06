using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.ProductTypes
{
    /// <summary>
    /// A data transfer object for requesting the update of a product type.
    /// </summary>
    public class UpdateProductTypeDto
    {
        /// <summary>
        /// Existing product type's identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Unique product type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The product type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
