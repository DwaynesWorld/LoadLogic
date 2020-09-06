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
    /// An immutable command message for requesting the deletion of a contact.
    /// </summary>
    public sealed class DeleteProfileContact : IRequest
    {
        public DeleteProfileContact(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The contact's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; private set; }
    }


    internal class DeleteProfileContactHandler : AsyncRequestHandler<DeleteProfileContact>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _repository;

        public DeleteProfileContactHandler(
            IMediator mediator,
            ICrudRepository<Profile> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(DeleteProfileContact request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _repository.FindOneAsync(spec, Profile.IncludeVendors);
            if (profile == null)
            {
                // FIXME: Profile Id
                throw new NotFoundException(nameof(Profile), 1);
            }

            profile.RemoveContact(request.Id);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
