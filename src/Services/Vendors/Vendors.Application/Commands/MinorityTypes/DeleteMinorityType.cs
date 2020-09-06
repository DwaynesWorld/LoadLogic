using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.MinorityTypes
{
    /// <summary>
    /// An immutable command message for requesting the deletion of a minority type.
    /// </summary>
    public sealed class DeleteMinorityType : IRequest
    {
        public DeleteMinorityType(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The existing minority type's identifier.
        /// </summary>
        [Required]
        public long Id { get; private set; }
    }

    internal class DeleteMinorityTypeHandler : IRequestHandler<DeleteMinorityType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<MinorityType> _repository;

        public DeleteMinorityTypeHandler(
            IMediator mediator,
            ICrudRepository<MinorityType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteMinorityType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(MinorityType), request.Id.ToString());
            }

            try
            {
                _repository.Remove(type);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new EntityDeletionException(nameof(MinorityType), request.Id.ToString());
                }

                throw;
            }

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
