
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.CompanyTypes
{
    /// <summary>
    /// An immutable command message for requesting the update of a company type.
    /// </summary>
    public sealed class UpdateCompanyType : IRequest
    {
        public UpdateCompanyType(long id, string code, string description)
        {
            this.Id = id;
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// The existing company type's identifier.
        /// </summary>
        [Required]
        public long Id { get; }

        /// <summary>
        /// A unique company type code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The company type's description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }

    internal class UpdateCompanyTypeHandler : IRequestHandler<UpdateCompanyType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<CompanyType> _repository;

        public UpdateCompanyTypeHandler(
            IMediator mediator,
            ICrudRepository<CompanyType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(UpdateCompanyType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(CompanyType), request.Id.ToString());
            }

            if (type.Code.ToUpper() != request.Code.ToUpper())
            {
                var spec = new UniqueCompanyTypeSpec(request.Code);
                var match = await _repository.FindAnyAsync(spec);
                if (match)
                {
                    throw new DuplicateCodeException(nameof(CompanyType), request.Code);
                }
            }

            type.Update(request.Code, request.Description);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
