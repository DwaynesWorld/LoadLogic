using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.ProductTypes
{
    /// <summary>
    /// A command for requesting the update of a product type.
    /// </summary>
    public sealed class UpdateProductType : IRequest
    {
        public UpdateProductType(long id, string code, string description)
        {
            this.Id = id;
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// Existing product type's identifier.
        /// </summary>
        [Required]
        public long Id { get; }

        /// <summary>
        /// Unique product type code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The product type's description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }

    internal class UpdateProductTypeHandler : IRequestHandler<UpdateProductType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<ProductType> _repository;

        public UpdateProductTypeHandler(
            IMediator mediator,
            ICrudRepository<ProductType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(UpdateProductType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(ProductType), request.Id.ToString());
            }

            if (type.Code.ToUpper() != request.Code.ToUpper())
            {
                var spec = new UniqueProductTypeSpec(request.Code);
                var match = await _repository.FindAnyAsync(spec);
                if (match)
                {
                    throw new DuplicateCodeException(nameof(ProductType), request.Code);
                }
            }

            type.Update(request.Code, request.Description);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
