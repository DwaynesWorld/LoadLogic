using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Regions
{
    /// <summary>
    /// An immutable command for requesting the deletion of a region.
    /// </summary>
    public sealed class DeleteRegion : IRequest
    {
        public DeleteRegion(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The existing region's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }
    }

    internal class DeleteRegionHandler : IRequestHandler<DeleteRegion>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Region> _repository;

        public DeleteRegionHandler(
            IMediator mediator,
            ICrudRepository<Region> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteRegion request, CancellationToken cancellationToken)
        {
            var region = await _repository.FindByIdAsync(request.Id);
            if (region == null)
            {
                throw new NotFoundException(nameof(Region), request.Id.ToString());
            }

            try
            {
                _repository.Remove(region);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new EntityDeletionException(nameof(Region), request.Id.ToString());
                }

                throw;
            }

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
