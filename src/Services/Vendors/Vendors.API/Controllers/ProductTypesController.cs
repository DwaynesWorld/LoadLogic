using LoadLogic.Services.Vendors.Application.Commands.ProductTypes;
using LoadLogic.Services.Vendors.Application.Models.ProductTypes;
using LoadLogic.Services.Vendors.Application.Queries.ProductTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing product types that have been associated
    /// with a particular company/business unit.  The response is scoped to the current user's
    /// company (i.e., a user adds/deletes/modifies product types on behalf of
    /// all users belonging to their company/business unit).
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/productTypes")]
    public class ProductTypesController : RootController
    {
        public ProductTypesController() { }

        /// <summary>
        /// Returns the product type specified by the unique identifier,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The product type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Product type not found.</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProductTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetProductTypeById(id);
            var product = await Mediator.Send(message, cancellationToken);
            return Ok(product);
        }

        /// <summary>
        /// Returns all product types, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ProductTypeDto>), 200)]
        public async Task<IActionResult> GetProductTypes(
            CancellationToken cancellationToken)
        {
            var message = new GetProductTypes();
            var products = await Mediator.Send(message, cancellationToken);
            return Ok(products);
        }

        /// <summary>
        /// Creates a new product type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Product type created.</response>
        /// <response code="409">Product type code already in use.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateProductType(
            [FromBody] CreateProductTypeDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateProductType(dto.Code, dto.Description);
            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetProductType), new { id }, id);
        }

        /// <summary>
        /// Updates an existing product type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Product type updated.</response>
        /// <response code="409">Product type code already in use.</response>
        /// <response code="404">Product type not found.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProductType(
            [FromRoute] long id,
            [FromBody] UpdateProductTypeDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateProductType(dto.Id, dto.Code, dto.Description);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing product type, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id">The product type's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Product type deleted.</response>
        /// <response code="404">Product type not found.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProductType(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteProductType(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
    }
}
