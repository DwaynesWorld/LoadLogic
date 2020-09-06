using LoadLogic.Services.Vendors.Application.Commands.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Models.MinorityTypes;
using LoadLogic.Services.Vendors.Application.Queries.MinorityTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing minority types that have been associated
    /// with a particular company/business unit. The response is scoped to the current user's
    /// company (i.e., a user adds/deletes/modifies minority types on behalf of
    /// all users belonging to their company/business unit).
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/minorityTypes")]
    public class MinorityTypesController : RootController
    {
        public MinorityTypesController() { }

        /// <summary>
        /// Returns the minority type specified by the unique identifier,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The minority type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Minority type not found.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(MinorityTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMinorityType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetMinorityTypeById(id);
            var type = await Mediator.Send(message, cancellationToken);
            return Ok(type);
        }

        /// <summary>
        /// Returns all minority types, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<MinorityTypeDto>), 200)]
        public async Task<IActionResult> GetMinorityTypes(CancellationToken cancellationToken)
        {
            var message = new GetMinorityTypes();
            var types = await Mediator.Send(message, cancellationToken);
            return Ok(types);
        }

        /// <summary>
        /// Creates a new minority type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Minority type created.</response>
        /// <response code="409">Minority type code already in use.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateMinorityType(
            [FromBody] CreateMinorityTypeDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateMinorityType(dto.Code, dto.Description);
            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetMinorityType), new { id }, id);
        }

        /// <summary>
        /// Updates an existing minority type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Minority type updated.</response>
        /// <response code="409">Minority type code already in use.</response>
        /// <response code="404">Minority type not found.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMinorityType(
            [FromRoute] long id,
            [FromBody] UpdateMinorityTypeDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateMinorityType(dto.Id, dto.Code, dto.Description);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing minority type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The minority type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Minority type deleted.</response>
        /// <response code="404">Minority type not found.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMinorityType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteMinorityType(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
    }
}
