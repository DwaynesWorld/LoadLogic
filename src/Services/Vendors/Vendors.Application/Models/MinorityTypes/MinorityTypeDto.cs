using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.MinorityTypes
{
    /// <summary>
    /// Minority type data transfer object.
    /// </summary>
    public class MinorityTypeDto
    {
        /// <summary>
        /// The Minority type's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Minority type code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Minority type description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
