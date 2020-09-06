
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.CompanyTypes
{
    /// <summary>
    /// An immutable command message for requesting the creation of a company type.
    /// </summary>
    public sealed class CreateCompanyType : IRequest<long>
    {
        public CreateCompanyType(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

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

    internal class CreateCompanyTypeHandler : IRequestHandler<CreateCompanyType, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<CompanyType> _repository;

        public CreateCompanyTypeHandler(IMediator mediator, ICrudRepository<CompanyType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<long> Handle(CreateCompanyType request, CancellationToken cancellationToken)
        {
            var spec = new UniqueCompanyTypeSpec(request.Code);
            var match = await _repository.FindAnyAsync(spec);
            if (match)
            {
                throw new DuplicateCodeException(nameof(CompanyType), request.Code);
            }

            var type = new CompanyType(request.Code, request.Description);
            _repository.Add(type);

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return type.Id;
        }
    }
}
