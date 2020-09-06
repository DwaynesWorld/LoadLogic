using System;
using System.ComponentModel.DataAnnotations;

namespace LoadLogic.Services.Vendors.Application.Models.MinorityTypes
{
    /// <summary>
    /// A data transfer object for requesting the update of a minority type.
    /// </summary>
    public class UpdateMinorityTypeDto
    {
        /// <summary>
        /// The existing minority type's identifier.
        /// </summary>
        [Required]
        public long Id { get; set; }

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
