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
    /// An immutable request for requesting the deletion of a company product.
    /// </summary>
    public sealed class DeleteProfileProduct : IRequest
    {
        public DeleteProfileProduct(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The company product's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }
    }

    internal class DeleteProfileProductHandler : AsyncRequestHandler<DeleteProfileProduct>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;

        public DeleteProfileProductHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
        }

        protected override async Task Handle(DeleteProfileProduct request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeProducts);
            if (profile == null)
            {
                // FIXME: Profile Id
                throw new NotFoundException(nameof(Profile), 1);
            }

            profile.RemoveProduct(request.Id);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
