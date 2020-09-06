using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.Profiles
{
    /// <summary>
    /// An immutable command message for requesting the deletion of a company's profile.
    /// </summary>
    public sealed class DeleteProfile : IRequest { }

    internal class DeleteProfileHandler : IRequestHandler<DeleteProfile>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _repository;

        public DeleteProfileHandler(
            IMediator mediator,
            ICrudRepository<Profile> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteProfile request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _repository.FindOneAsync(spec);
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), default);
            }

            _repository.Remove(profile);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
