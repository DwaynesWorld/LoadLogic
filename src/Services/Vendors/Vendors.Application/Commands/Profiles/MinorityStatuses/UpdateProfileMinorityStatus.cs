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
    /// An immutable command message for requesting an update of a profile's minority status.
    /// </summary>
    public sealed class UpdateProfileMinorityStatus : IRequest
    {
        public UpdateProfileMinorityStatus(long id, long minorityTypeId, string certificationNumber, decimal percent)
        {
            this.Id = id;
            this.MinorityTypeId = minorityTypeId;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }


        /// <summary>
        /// The profile minority status's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

        /// <summary>
        /// The minority status's type unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; }

        /// <summary>
        /// The minority status certification number.
        /// </summary>
        public string CertificationNumber { get; } = string.Empty;

        /// <summary>
        /// The minority percentage.
        /// </summary>
        public decimal Percent { get; }
    }

    internal class UpdateProfileMinorityStatusHandler : AsyncRequestHandler<UpdateProfileMinorityStatus>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly ICrudRepository<MinorityType> _minorityTypeRepo;

        public UpdateProfileMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo,
            ICrudRepository<MinorityType> minorityTypeRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
            _minorityTypeRepo = minorityTypeRepo ?? throw new ArgumentNullException(nameof(minorityTypeRepo));
        }

        protected override async Task Handle(UpdateProfileMinorityStatus request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeStatuses);
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), default);
            }

            var status = profile.FindMinorityStatusById(request.Id);

            var type = status.Type;
            if (type == null || type.Id != request.MinorityTypeId)
            {
                type = await _minorityTypeRepo.FindByIdAsync(request.MinorityTypeId);
                if (type == null)
                {
                    throw new NotFoundException(nameof(MinorityType), request.MinorityTypeId);
                }
            }

            status.Update(type, request.CertificationNumber, request.Percent);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
