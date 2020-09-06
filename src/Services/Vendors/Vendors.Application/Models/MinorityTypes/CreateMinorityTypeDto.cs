using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.MinorityTypes
{
    /// <summary>
    /// A data transfer object for requesting the creation of a minority type.
    /// </summary>
    public class CreateMinorityTypeDto
    {
        /// <summary>
        /// A unique minority type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The minority type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
