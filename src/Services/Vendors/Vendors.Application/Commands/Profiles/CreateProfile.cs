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
    /// An immutable command message for requesting the creation of a company profile.
    /// </summary>
    public sealed class CreateProfile : IRequest<long>
    {
        public CreateProfile(
            string name, Address? primaryAddress, Address? alternateAddress,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber,
            string webAddress, long? regionId, CommunicationMethod communicationMethod,
            string profileAccentColor)
        {
            this.Name = name;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.RegionId = regionId;
            this.CommunicationMethod = communicationMethod;
            this.ProfileAccentColor = profileAccentColor;
        }

        /// <summary>
        /// The company's name.
        /// </summary>
        [Required]
        public string Name { get; } = string.Empty;

        /// <summary>
        /// The company's primary address.
        /// </summary>
        public Address? PrimaryAddress { get; }

        /// <summary>
        /// The company's alternate address.
        /// </summary>
        public Address? AlternateAddress { get; }

        public PhoneNumber? PhoneNumber { get; }

        public PhoneNumber? FaxNumber { get; }

        /// <summary>
        /// The company's website address.
        /// </summary>
        public string WebAddress { get; } = string.Empty;

        /// <summary>
        /// The unique identifier of the region the company operates in.
        /// </summary>
        public long? RegionId { get; }

        /// <summary>
        /// The best form of communication of contacting this company.
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; }

        /// <summary>
        /// The accent color for the company.
        /// </summary>
        public string ProfileAccentColor { get; }

    }

    internal class CreateProfileHandler : IRequestHandler<CreateProfile, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public CreateProfileHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo,
            ICrudRepository<Region> regionRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
            _regionRepo = regionRepo ?? throw new ArgumentNullException(nameof(regionRepo));
        }

        public async Task<long> Handle(CreateProfile request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var match = await _profileRepo.FindAnyAsync(spec);
            if (match)
            {
                // FIXME: Profile Id
                throw new DuplicateProfileException(1);
            }

            Region? region = null;
            if (request.RegionId.HasValue)
            {
                region = await _regionRepo.FindByIdAsync(request.RegionId.Value);
                if (region == null)
                {
                    throw new NotFoundException(nameof(Region), request.RegionId.Value);
                }
            }

            var profile = new Profile(
                request.Name,
                request.PrimaryAddress,
                request.AlternateAddress,
                request.PhoneNumber,
                request.FaxNumber,
                request.WebAddress, region,
                request.CommunicationMethod,
                request.ProfileAccentColor);

            _profileRepo.Add(profile);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return profile.Id;
        }
    }
}
