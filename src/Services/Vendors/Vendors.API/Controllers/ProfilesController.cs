using LoadLogic.Services.Vendors.Application.Commands.Profiles;
using LoadLogic.Services.Vendors.Application.Models.Profiles;
using LoadLogic.Services.Vendors.Application.Queries.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.API.Controllers
{
    /// <summary>
    /// A controller for accessing public profiles.
    /// Profiles are read accessible to any authenticated user.
    /// Profiles are write accessible to any authenticated user of the respective company.
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/profiles")]
    public class ProfilesController : RootController
    {
        public ProfilesController() { }

        #region Profile
        /// <summary>
        /// Returns the profile for the current authenticated user's company/business unit.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Profile not found</response>
        [HttpGet]
        [Route("me")]
        [ProducesResponseType(typeof(ProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var message = new GetCurrentUserProfile();
            var profile = await Mediator.Send(message, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Returns the profile specified by the unique identifier.
        /// </summary>
        /// <param name="id">The profile's unique identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Profile not found</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ProfileDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfile(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new GetProfileById(id);
            var profile = await Mediator.Send(message, cancellationToken);
            return Ok(profile);
        }

        /// <summary>
        /// Returns all profiles.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<ProfileSummaryDto>), 200)]
        public async Task<IActionResult> GetProfiles(CancellationToken cancellationToken)
        {
            var message = new GetProfiles();
            var profiles = await Mediator.Send(message, cancellationToken);
            return Ok(profiles);
        }

        /// <summary>
        /// Creates a new profile, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created the profile.</response>
        /// <response code="409">Profile already exists.</response>
        /// <response code="404">Region not found.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateProfile(
            [FromBody] CreateProfileDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateProfile(
                dto.Name, dto.PrimaryAddress, dto.AlternateAddress,
                (PhoneNumber)dto.PhoneNumber, (PhoneNumber)dto.FaxNumber,
                dto.WebAddress, dto.RegionId, dto.CommunicationMethod, dto.ProfileAccentColor);

            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetProfile), new { id }, id);
        }

        /// <summary>
        /// Adds a company logo to a storage and updates profile with logo url. 
        /// This method will overwrite any existing logos.
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Successfully added the logo.</response>
        /// <response code="413">Invalid content length.</response>
        /// <response code="404">Profile not found.</response>
        /// <response code="415">Invalid content type.</response>
        [HttpPost]
        [Route("logo")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(413)]
        [ProducesResponseType(415)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProfileLogo(
            IFormFile logo,
            CancellationToken cancellationToken)
        {
            var message = new AddProfileLogo(logo.ContentType, logo.Length, logo.OpenReadStream());
            var url = await Mediator.Send(message, cancellationToken);
            return Ok(url);
        }

        /// <summary>
        /// Updates a profile, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated the profile.</response>
        /// <response code="404">Profile/Region not found.</response>
        [HttpPut]
        [Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileDto dto,
            CancellationToken cancellationToken)
        {
            var message = new UpdateProfile(
                dto.Name, dto.PrimaryAddress, dto.AlternateAddress,
                (PhoneNumber)dto.PhoneNumber, (PhoneNumber)dto.FaxNumber,
                dto.WebAddress, dto.RegionId, dto.CommunicationMethod, dto.ProfileAccentColor);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes a profile, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <response code="404">Profile not found.</response>
        /// <response code="204">Successfully deleted the profile.</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProfile(CancellationToken cancellationToken)
        {
            var message = new DeleteProfile();
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion

        #region Vendors
        /// <summary>
        /// Adds a contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created contact.</response>
        /// <response code="404">Profile not found.</response>
        [HttpPost]
        [Route("contacts")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateProfileContact(
            [FromBody] CreateProfileContactDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateProfileContact(
                dto.FirstName, dto.LastName, dto.Title,
                dto.PhoneNumber, dto.FaxNumber, dto.CellPhoneNumber,
                dto.EmailAddress, dto.Note, dto.IsMainContact);

            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetMyProfile), new { }, id);
        }

        /// <summary>
        /// Updates the specified contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated contact.</response>
        /// <response code="404">Profile/Contact not found.</response>
        [HttpPut]
        [Route("contacts/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfileContact(
            [FromRoute] long id,
            [FromBody] UpdateProfileContactDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateProfileContact(
                dto.Id, dto.FirstName, dto.LastName, dto.Title,
                dto.PhoneNumber, dto.FaxNumber, dto.CellPhoneNumber,
                dto.EmailAddress, dto.Note, dto.IsMainContact);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified contact, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted contact.</response>
        /// <response code="404">Profile/Contact not found.</response>
        [HttpDelete]
        [Route("contacts/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProfileContact(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteProfileContact(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion

        #region Minority Statuses
        /// <summary>
        /// Creates a minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created minority status.</response>
        /// <response code="404">Profile/Minority Type not found.</response>
        [HttpPost]
        [Route("statuses")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateProfileMinorityStatus(
            [FromBody] CreateProfileMinorityStatusDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateProfileMinorityStatus(
                dto.MinorityTypeId,
                dto.CertificationNumber,
                dto.Percent);

            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetMyProfile), new { }, id);
        }

        /// <summary>
        /// Updates the specified minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated minority status.</response>
        /// <response code="404">Profile/Minority Status/Type not found.</response>
        [HttpPut]
        [Route("statuses/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfileMinoirtyStatus(
            [FromRoute] long id,
            [FromBody] UpdateProfileMinorityStatusDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateProfileMinorityStatus(
                dto.Id,
                dto.MinorityTypeId,
                dto.CertificationNumber,
                dto.Percent);

            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified minority status, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted status.</response>
        /// <response code="404">Profile/Minority Status not found.</response>
        [HttpDelete]
        [Route("statuses/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProfileMinorityStatus(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteProfileMinorityStatus(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion

        #region Products
        /// <summary>
        /// Adds a product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="201">Successfully created product.</response>
        /// <response code="404">Profile/Product not found.</response>
        [HttpPost]
        [Route("products")]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateProfileProduct(
            [FromBody] CreateProfileProductDto dto,
            CancellationToken cancellationToken)
        {
            var message = new CreateProfileProduct(dto.ProductTypeId, dto.RegionId);
            var id = await Mediator.Send(message, cancellationToken);
            return CreatedAtAction(nameof(GetMyProfile), new { }, id);
        }

        /// <summary>
        /// Updates the specified product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully updated product.</response>
        /// <response code="404">Profile/Product not found.</response>
        [HttpPut]
        [Route("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateProfileProduct(
            [FromRoute] long id,
            [FromBody] UpdateProfileProductDto dto,
            CancellationToken cancellationToken)
        {
            if (id != dto.Id)
            {
                throw new InvalidRequestException(id, dto.Id);
            }

            var message = new UpdateProfileProduct(dto.Id, dto.ProductTypeId, dto.RegionId);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes the specified product, scoped to the company/business unit of the current user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <response code="204">Successfully deleted product.</response>
        /// <response code="404">Profile/Product not found.</response>
        [HttpDelete]
        [Route("products/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProfileProduct(
            [FromRoute] long id,
            CancellationToken cancellationToken)
        {
            var message = new DeleteProfileProduct(id);
            await Mediator.Send(message, cancellationToken);
            return NoContent();
        }
        #endregion
    }
}
