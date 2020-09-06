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
    /// An immutable command message for requesting the creation of an company minority status.
    /// </summary>
    public sealed class CreateProfileMinorityStatus : IRequest<long>
    {
        public CreateProfileMinorityStatus(long minorityTypeId, string certificationNumber, decimal percent)
        {
            this.MinorityTypeId = minorityTypeId;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

        /// <summary>
        /// The minority status type's unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; }

        /// <summary>
        /// The company's minority certification number.
        /// </summary>
        public string CertificationNumber { get; } = string.Empty;

        /// <summary>
        /// The company's minority percentage.
        /// </summary>
        public decimal Percent { get; }
    }


    internal class CreateProfileMinorityStatusHandler : IRequestHandler<CreateProfileMinorityStatus, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly ICrudRepository<MinorityType> _minorityTypeRepo;

        public CreateProfileMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo,
            ICrudRepository<MinorityType> minorityTypeRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
            _minorityTypeRepo = minorityTypeRepo ?? throw new ArgumentNullException(nameof(minorityTypeRepo));
        }

        public async Task<long> Handle(CreateProfileMinorityStatus request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeStatuses);
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), default);
            }

            var type = await _minorityTypeRepo.FindByIdAsync(request.MinorityTypeId);
            if (type == null)
            {
                throw new NotFoundException(nameof(MinorityType), request.MinorityTypeId);
            }

            var status = new ProfileMinorityStatus(
                profile,
                type,
                request.CertificationNumber,
                request.Percent);

            profile.AddMinorityStatus(status);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return status.Id;
        }
    }
}
