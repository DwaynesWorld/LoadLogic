using LoadLogic.Services.Vendors.Application.Commands.Regions;
using LoadLogic.Services.Vendors.Application.Models.Regions;
using LoadLogic.Services.Vendors.Application.Queries.Regions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing regions that have been associated
    /// with a particular company/business unit.  The response is scoped to the current user's
    /// company (i.e., a user adds/deletes/modifies regions on behalf of
    /// all users belonging to their company/business unit).
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/regions")]
    public class RegionsController : RootController
    {
        public RegionsController() { }

        /// <summary>
        /// Returns the region specified by the unique identifier,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The Region's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Region not found.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RegionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRegion(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetRegionById(id);
            var region = await Mediator.Send(message, cancellationToken);
            return Ok(region);
        }

        /// <summary>
        /// Returns all regions, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<RegionDto>), 200)]
        public async Task<IActionResult> GetRegions(CancellationToken cancellationToken)
        {
            var message = new GetRegions();
            var regions = await Mediator.Send(message, cancellationToken);
            return Ok(regions);
        }

        /// <summary>
        /// Creates a new region, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Region created.</response>
        /// <response code="409">Region code already in use.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateRegion(
            [FromBody] CreateRegionDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateRegion(dto.Code, dto.Description);
            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetRegion), new { id }, id);
        }

        /// <summary>
        /// Updates an existing region, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Region updated.</response>
        /// <response code="409">Region code already in use.</response>
        /// <response code="404">Region not found.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRegion(
            [FromRoute] long id,
            [FromBody] UpdateRegionDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateRegion(dto.Id, dto.Code, dto.Description);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing region, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The region's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Region deleted.</response>
        /// <response code="404">Region not found.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRegion(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteRegion(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
    }
}
