using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.ProductTypes
{
    /// <summary>
    /// A Product Type data transfer object.
    /// </summary>
    public class ProductTypeDto
    {
        /// <summary>
        /// The Product type's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Product type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Product type description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
