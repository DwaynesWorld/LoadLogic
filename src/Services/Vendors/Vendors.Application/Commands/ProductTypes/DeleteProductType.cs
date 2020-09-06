using System.ComponentModel.DataAnnotations;

using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.ProductTypes
{

    /// <summary>
    /// An immutable command for requesting the deletion of a product type.
    /// </summary>
    public sealed class DeleteProductType : IRequest
    {
        public DeleteProductType(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The existing product identifier.
        /// </summary>
        [Required]
        public long Id { get; private set; }
    }


    internal class DeleteProductTypeHandler : IRequestHandler<DeleteProductType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<ProductType> _repository;

        public DeleteProductTypeHandler(
            IMediator mediator,
            ICrudRepository<ProductType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteProductType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(ProductType), request.Id.ToString());
            }

            try
            {
                _repository.Remove(type);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new EntityDeletionException(nameof(ProductType), request.Id.ToString());
                }

                throw;
            }

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
