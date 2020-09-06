using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.CompanyTypes
{
    /// <summary>
    /// A data transfer object for requesting the update of a company type.
    /// </summary>
    public class UpdateCompanyTypeDto
    {
        /// <summary>
        /// The existing company type's identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// A unique company type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The company type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
