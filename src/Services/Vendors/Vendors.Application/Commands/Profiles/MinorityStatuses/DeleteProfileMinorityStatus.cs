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
    /// An immutable command message for requesting the deletion of an company minority status.
    /// </summary>
    public sealed class DeleteProfileMinorityStatus : IRequest
    {
        public DeleteProfileMinorityStatus(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The company minority status's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; private set; }
    }

    internal class DeleteProfileMinorityStatusHandler : AsyncRequestHandler<DeleteProfileMinorityStatus>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;

        public DeleteProfileMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
        }

        protected override async Task Handle(DeleteProfileMinorityStatus request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeStatuses);
            if (profile == null)
            {
                // FIXME: Profile Id
                throw new NotFoundException(nameof(Profile), 1);
            }

            profile.RemoveStatus(request.Id);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
