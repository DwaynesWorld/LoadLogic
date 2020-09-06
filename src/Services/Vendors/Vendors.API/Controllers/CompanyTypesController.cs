using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Models.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Commands.CompanyTypes;
using LoadLogic.Services.Vendors.Application.Queries.CompanyTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing company types that have been associated
    /// with a particular company/business unit. The response is scoped to the current user's
    /// company (i.e., a user adds/deletes/modifies company types on behalf of
    /// all users belonging to their company/business unit).
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/companyTypes")]
    public class CompanyTypesController : RootController
    {
        public CompanyTypesController() { }

        /// <summary>
        /// Returns the company type specified by the unique identifier,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The company type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Company type not found.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CompanyTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetCompanyTypeById(id);
            var type = await Mediator.Send(message, cancellationToken);
            return Ok(type);
        }

        /// <summary>
        /// Returns all company types, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<CompanyTypeDto>), 200)]
        public async Task<IActionResult> GetCompanyTypes(CancellationToken cancellationToken)
        {
            var message = new GetCompanyTypes();
            var types = await Mediator.Send(message, cancellationToken);
            return Ok(types);
        }

        /// <summary>
        /// Creates a new company type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto">The new company type</param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Company type created.</response>
        /// <response code="409">Company type Code already in use.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateCompanyType(
            [FromBody] CreateCompanyTypeDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateCompanyType(dto.Code, dto.Description);
            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetCompanyType), new { id }, id);
        }

        /// <summary>
        /// Updates an existing company type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The company type's unique identifier.</param>
        /// <param name="dto">The updated company type.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Company type updated.</response>
        /// <response code="409">Company type code already in use.</response>
        /// <response code="404">Company type not found.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCompanyType(
            [FromRoute] long id,
            [FromBody] UpdateCompanyTypeDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateCompanyType(dto.Id, dto.Code, dto.Description);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing company type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The company type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Company type deleted.</response>
        /// <response code="404">Company type not found.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCompanyType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteCompanyType(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
    }
}
