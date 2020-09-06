using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.ProductTypes
{
    /// <summary>
    /// An immutable command message for requesting the creation of a product type.
    /// </summary>
    public sealed class CreateProductType : IRequest<long>
    {
        public CreateProductType(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// A unique product type code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The product type's description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }

    internal class CreateProductTypeHandler : IRequestHandler<CreateProductType, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<ProductType> _repository;

        public CreateProductTypeHandler(
            IMediator mediator,
            ICrudRepository<ProductType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<long> Handle(CreateProductType request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProductTypeSpec(request.Code);
            var match = await _repository.FindAnyAsync(spec);
            if (match)
            {
                throw new DuplicateCodeException(nameof(ProductType), request.Code);
            }

            var type = new ProductType(request.Code, request.Description);

            _repository.Add(type);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return type.Id;
        }
    }
}
