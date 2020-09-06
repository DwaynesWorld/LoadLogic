using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.Regions
{
    /// <summary>
    /// Aa immutable command for requesting the creation of a region.
    /// </summary>
    public sealed class CreateRegion : IRequest<long>
    {
        public CreateRegion(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// Unique Region code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The Region description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }


    internal class CreateRegionHandler : IRequestHandler<CreateRegion, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Region> _repository;

        public CreateRegionHandler(
            IMediator mediator,
            ICrudRepository<Region> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<long> Handle(CreateRegion request, CancellationToken cancellationToken)
        {
            var spec = new UniqueRegionSpec(request.Code);
            var match = await _repository.FindAnyAsync(spec);
            if (match)
            {
                throw new DuplicateCodeException(nameof(Region), request.Code);
            }

            var region = new Region(request.Code, request.Description);
            _repository.Add(region);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return region.Id;
        }
    }
}
