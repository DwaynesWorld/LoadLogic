using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Vendors
{
    /// <summary>
    /// An immutable command message for requesting an update to a contact.
    /// </summary>
    public sealed class UpdateVendorContact : IRequest
    {
        public UpdateVendorContact(
            long vendorId, long contactId, string firstName, string lastName,
            string title, string phoneNumber, string faxNumber,
            string cellPhoneNumber, string emailAddress, string note, bool isMainContact)
        {
            this.VendorId = vendorId;
            this.ContactId = contactId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Title = title;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.CellPhoneNumber = cellPhoneNumber;
            this.EmailAddress = emailAddress;
            this.Note = note;
            this.IsMainContact = isMainContact;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The contact's unique identifier.
        /// </summary>
        [Required]
        public long ContactId { get; }

        /// <summary>
        /// The contact's first name.
        /// </summary>
        public string FirstName { get; } = string.Empty;

        /// <summary>
        /// The contact's last name.
        /// </summary>
        public string LastName { get; } = string.Empty;

        /// <summary>
        /// The contact's title.
        /// </summary>
        public string Title { get; } = string.Empty;

        /// <summary>
        /// The contact's phone number.
        /// </summary>
        public string PhoneNumber { get; } = string.Empty;

        /// <summary>
        /// The contact's fax number.
        /// </summary>
        public string FaxNumber { get; } = string.Empty;

        /// <summary>
        /// The contact's cell phone number.
        /// </summary>
        public string CellPhoneNumber { get; } = string.Empty;

        /// <summary>
        /// The contact's email address.
        /// </summary>
        public string EmailAddress { get; } = string.Empty;

        /// <summary>
        /// The contact's note.
        /// </summary>
        public string Note { get; } = string.Empty;

        /// <summary>
        /// A boolean indicating whether this contact is the company's main contact.
        /// </summary>
        public bool IsMainContact { get; }
    }

    internal class UpdateVendorContactHandler : IRequestHandler<UpdateVendorContact>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;

        public UpdateVendorContactHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
        }

        public async Task<Unit> Handle(UpdateVendorContact request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeVendors);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            var contact = vendor.FindContactById(request.ContactId);

            if (request.IsMainContact) vendor.ClearCurrentMainContact();

            contact.Update(
                request.FirstName, request.LastName, request.Title,
                string.IsNullOrWhiteSpace(request.PhoneNumber) ? new PhoneNumber() : new PhoneNumber(request.PhoneNumber),
                string.IsNullOrWhiteSpace(request.FaxNumber) ? new PhoneNumber() : new PhoneNumber(request.FaxNumber),
                string.IsNullOrWhiteSpace(request.CellPhoneNumber) ? new PhoneNumber() : new PhoneNumber(request.CellPhoneNumber),
                string.IsNullOrWhiteSpace(request.EmailAddress) ? new Email() : new Email(request.EmailAddress),
                request.Note, request.IsMainContact);

            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
