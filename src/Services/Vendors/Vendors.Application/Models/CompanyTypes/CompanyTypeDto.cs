using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.CompanyTypes
{
    /// <summary>
    /// A company type data transfer object.
    /// </summary>
    public class CompanyTypeDto
    {
        /// <summary>
        /// The type's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// The type's unique code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The type's description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
