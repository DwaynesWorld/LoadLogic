using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;

namespace LoadLogic.Services.Vendors.Application.Models.Regions
{
    /// <summary>
    /// A data transfer object for requesting the creation of a region.
    /// </summary>
    public class CreateRegionDto
    {
        /// <summary>
        /// Unique Region code.
        /// </summary>
        [Required]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The Region description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
