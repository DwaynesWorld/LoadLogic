using System.ComponentModel.DataAnnotations;
using MediatR;
using LoadLogic.Services.Vendors.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Regions
{
    /// <summary>
    /// An immutable command for requesting the update of a Region.
    /// </summary>
    public sealed class UpdateRegion : IRequest
    {
        public UpdateRegion(long id, string code, string description)
        {
            this.Id = id;
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// The existing region's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

        /// <summary>
        /// The the region's unique code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The region's description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }

    internal class UpdateRegionHandler : IRequestHandler<UpdateRegion>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Region> _repository;

        public UpdateRegionHandler(
            IMediator mediator,
            ICrudRepository<Region> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(UpdateRegion request, CancellationToken cancellationToken)
        {
            var region = await _repository.FindByIdAsync(request.Id);
            if (region == null)
            {
                throw new NotFoundException(nameof(Region), request.Id.ToString());
            }

            if (region.Code.ToUpper() != request.Code.ToUpper())
            {
                var spec = new UniqueRegionSpec(request.Code);

                var match = await _repository.FindAnyAsync(spec);
                if (match)
                {
                    throw new DuplicateCodeException(nameof(Region), request.Code);
                }
            }

            region.Update(request.Code, request.Description);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
