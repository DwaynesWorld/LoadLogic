using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Profiles
{
    /// <summary>
    /// An immutable command message for requesting an update to a contact.
    /// </summary>
    public sealed class UpdateProfileContact : IRequest
    {
        public UpdateProfileContact(
            long id, string firstName, string lastName, string title,
            string phoneNumber, string faxNumber, string cellPhoneNumber,
            string emailAddress, string note, bool isMainContact)
        {
            this.Id = id;
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
        /// The contact's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

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

    internal class UpdateProfileContactHandler : AsyncRequestHandler<UpdateProfileContact>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;

        public UpdateProfileContactHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
        }

        protected override async Task Handle(UpdateProfileContact request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeVendors);
            if (profile == null)
            {
                // FIXME: Profile Id
                throw new NotFoundException(nameof(Profile), 1);
            }

            var contact = profile.FindContactById(request.Id);

            contact.Update(
                request.FirstName, request.LastName, request.Title,
                string.IsNullOrWhiteSpace(request.PhoneNumber) ? new PhoneNumber() : new PhoneNumber(request.PhoneNumber),
                string.IsNullOrWhiteSpace(request.FaxNumber) ? new PhoneNumber() : new PhoneNumber(request.FaxNumber),
                string.IsNullOrWhiteSpace(request.CellPhoneNumber) ? new PhoneNumber() : new PhoneNumber(request.CellPhoneNumber),
                string.IsNullOrWhiteSpace(request.EmailAddress) ? new Email() : new Email(request.EmailAddress),
                request.Note, request.IsMainContact);

            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
