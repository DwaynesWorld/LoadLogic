using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.CompanyTypes
{
    /// <summary>
    /// An data transfer object for requesting the creation of a company type.
    /// </summary>
    public class CreateCompanyTypeDto
    {
        /// <summary>
        /// The unique company type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The company type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
