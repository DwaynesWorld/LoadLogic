using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Vendors.Application.Commands.Vendors;
using LoadLogic.Services.Vendors.Application.Models.Vendors;
using LoadLogic.Services.Vendors.Application.Queries.Vendors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing vendors that have been associated
    /// with a particular company/business unit.  The response is scoped to the current user's
    /// company (i.e., a user adds/deletes/modifies vendors on behalf of
    /// all users belonging to their company/business unit).
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/vendors")]
    public class VendorsController : RootController
    {
        public VendorsController() { }

        #region Vendor
        /// <summary>
        /// Returns the vendor specified by the unique identifier,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Vendor not found</response>
        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(VendorDto), 200)]
        public async Task<IActionResult> GetVendor(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetVendorById(id);
            var vendor = await Mediator.Send(message, cancellationToken);
            return Ok(vendor);
        }


        /// <summary>
        /// Returns the vendor specified by the unique code,
        /// scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Vendor not found</response>
        [HttpGet]
        [Route("{code}")]
        [ProducesResponseType(typeof(VendorDto), 200)]
        public async Task<IActionResult> GetVendorByCode(
            [FromRoute] string code,
            CancellationToken cancellationToken)
        {
            var message = new GetVendorByCode(code);
            var vendor = await Mediator.Send(message, cancellationToken);
            return Ok(vendor);
        }


        /// <summary>
        /// Returns all vendors, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<VendorSummaryDto>), 200)]
        public async Task<IActionResult> GetVendors(CancellationToken cancellationToken)
        {
            var message = new GetVendors();
            var vendors = await Mediator.Send(message, cancellationToken);
            return Ok(vendors);
        }

        /// <summary>
        /// Returns all vendors with details, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("detailed")]
        [ProducesResponseType(typeof(IEnumerable<VendorDto>), 200)]
        public async Task<IActionResult> GetDetailedVendors(CancellationToken cancellationToken)
        {
            var message = new GetDetailedVendors();
            var vendors = await Mediator.Send(message, cancellationToken);
            return Ok(vendors);
        }

        /// <summary>
        /// Creates a new vendor, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created the vendor.</response>
        /// <response code="404">Region/Company type not found.</response>
        /// <response code="409">Vendor code already exists.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateVendor(
            [FromBody] CreateVendorDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateVendor(
               dto.Code, dto.Name, dto.TypeId,
               dto.PrimaryAddress, dto.AlternateAddress,
               (PhoneNumber)dto.PhoneNumber, (PhoneNumber)dto.FaxNumber, dto.WebAddress,
               dto.RegionId, dto.CommunicationMethod, dto.IsBonded,
               dto.BondRate, dto.Note);

            var vendorId = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetVendor), new { id = vendorId }, vendorId);
        }

        /// <summary>
        /// Updates an existing vendor, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated the vendor.</response>
        /// <response code="404">Vendor/Region not found.</response>
        /// <response code="409">Vendor code already exists.</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> UpdateVendor(
            [FromRoute] long id,
            [FromBody] UpdateVendorDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateVendor(
                dto.Id, dto.Code, dto.Name, dto.TypeId,
                dto.PrimaryAddress, dto.AlternateAddress,
                (PhoneNumber)dto.PhoneNumber, (PhoneNumber)dto.FaxNumber, dto.WebAddress,
                dto.RegionId, dto.CommunicationMethod, dto.IsBonded,
                dto.BondRate, dto.Note);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing vendor, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted the vendor.</response>
        /// <response code="404">Vendor not found.</response>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteVendor(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteVendor(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion


        #region Vendors
        /// <summary>
        /// Adds a contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created contact.</response>
        /// <response code="404">Vendor not found.</response>
        [HttpPost]
        [Route("{vendorId}/contacts")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVendorContact(
            [FromRoute] long vendorId,
            [FromBody] CreateVendorContactDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            var message = new CreateVendorContact(
                dto.VendorId, dto.FirstName, dto.LastName,
                dto.Title, dto.PhoneNumber, dto.FaxNumber, dto.CellPhoneNumber,
                dto.EmailAddress, dto.Note, dto.IsMainContact);

            var contactId = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetVendor), new { id = dto.VendorId }, contactId);
        }

        /// <summary>
        /// Updates the specified contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="contactId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated contact.</response>
        /// <response code="404">Vendor/Contact not found.</response>
        [HttpPut]
        [Route("{vendorId}/contacts/{contactId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVendorContact(
            [FromRoute] long vendorId,
            [FromRoute] long contactId,
            [FromBody] UpdateVendorContactDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            if (contactId != dto.ContactId)
            {
                throw new InvalidRequestException(contactId, dto.ContactId);
            }

            var message = new UpdateVendorContact(
                dto.VendorId, dto.ContactId, dto.FirstName, dto.LastName,
                dto.Title, dto.PhoneNumber, dto.FaxNumber, dto.CellPhoneNumber,
                dto.EmailAddress, dto.Note, dto.IsMainContact);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="contactId"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted contact.</response>
        /// <response code="404">Vendor/Contact not found.</response>
        [HttpDelete]
        [Route("{vendorId}/contacts/{contactId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVendorContact(
            [FromRoute] long vendorId,
            [FromRoute] long contactId,
            CancellationToken cancellationToken)
        {
            var message = new DeleteVendorContact(vendorId, contactId);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion

        #region Minority statuss
        /// <summary>
        /// Creates a minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created minority status.</response>
        /// <response code="404">Vendor/Minority status not found.</response>
        [HttpPost]
        [Route("{vendorId}/statuses")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVendorMinorityStatus(
            [FromRoute] long vendorId,
            [FromBody] CreateVendorMinorityStatusDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            var message = new CreateVendorMinorityStatus(
                dto.VendorId, dto.MinorityTypeId,
                dto.CertificationNumber, dto.Percent);

            var statusId = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetVendor), new { id = dto.VendorId }, statusId);
        }

        /// <summary>
        /// Updates the specified minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="statusId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated minority status.</response>
        /// <response code="404">Vendor/Minority status not found.</response>
        [HttpPut]
        [Route("{vendorId}/statuses/{statusId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVendorMinorityStatus(
            [FromRoute] long vendorId,
            [FromRoute] long statusId,
            [FromBody] UpdateVendorMinorityStatusDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            if (statusId != dto.StatusId)
            {
                throw new InvalidRequestException(statusId, dto.StatusId);
            }

            var message = new UpdateVendorMinorityStatus(
                dto.VendorId, dto.StatusId, dto.MinorityTypeId,
                dto.CertificationNumber, dto.Percent);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="statusId"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted minority status.</response>
        /// <response code="404">Vendor/Minority status not found.</response>
        [HttpDelete]
        [Route("{vendorId}/statuses/{statusId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVendorMinorityStatus(
            [FromRoute] long vendorId,
            [FromRoute] long statusId,
            CancellationToken cancellationToken)
        {
            var message = new DeleteVendorMinorityStatus(vendorId, statusId);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion

        #region Products
        /// <summary>
        /// Adds a product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created product.</response>
        /// <response code="404">Vendor/Product not found.</response>
        [HttpPost]
        [Route("{vendorId}/products")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateVendorProduct(
            [FromRoute] long vendorId,
            [FromBody] CreateVendorProductDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            var message = new CreateVendorProduct(
                dto.VendorId, dto.ProductTypeId, dto.RegionId);

            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetVendor), new { id = dto.VendorId }, id);
        }

        /// <summary>
        /// Updates the specified product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="productId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated product.</response>
        /// <response code="404">Vendor/Product not found.</response>
        [HttpPut]
        [Route("{vendorId}/products/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVendorProduct(
            [FromRoute] long vendorId,
            [FromRoute] long productId,
            [FromBody] UpdateVendorProductDto dto,
            CancellationToken cancellationToken)
        {
            if (vendorId != dto.VendorId)
            {
                throw new InvalidRequestException(vendorId, dto.VendorId);
            }

            if (productId != dto.ProductId)
            {
                throw new InvalidRequestException(productId, dto.ProductId);
            }

            var message = new UpdateVendorProduct(
                dto.VendorId, dto.ProductId, dto.ProductTypeId, dto.RegionId);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="productId"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted product.</response>
        /// <response code="404">Vendor/Product not found.</response>
        [HttpDelete]
        [Route("{vendorId}/products/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteVendorProduct(
            [FromRoute] long vendorId,
            [FromRoute] long productId,
            CancellationToken cancellationToken)
        {
            var message = new DeleteVendorProduct(vendorId, productId);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion
    }
}
