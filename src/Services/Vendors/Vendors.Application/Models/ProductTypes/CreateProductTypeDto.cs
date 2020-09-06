using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.ProductTypes
{
    /// <summary>
    /// A data transfer object for requesting the creation of a product type.
    /// </summary>
    public class CreateProductTypeDto
    {
        /// <summary>
        /// A unique product type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The product type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
