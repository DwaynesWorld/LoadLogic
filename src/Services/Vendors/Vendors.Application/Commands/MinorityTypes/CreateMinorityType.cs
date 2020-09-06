using System.ComponentModel.DataAnnotations;

using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.MinorityTypes
{
    /// <summary>
    /// An immutable command message for requesting the creation of a minority type.
    /// </summary>
    public sealed class CreateMinorityType : IRequest<long>
    {
        public CreateMinorityType(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }


        /// <summary>
        /// A unique minority type code.
        /// </summary>
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The minority type's description.
        /// </summary>
        public string Description { get; } = string.Empty;
    }

    internal class CreateMinorityTypeHandler : IRequestHandler<CreateMinorityType, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<MinorityType> _repository;

        public CreateMinorityTypeHandler(
            IMediator mediator,
            ICrudRepository<MinorityType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<long> Handle(CreateMinorityType request, CancellationToken cancellationToken)
        {
            var spec = new UniqueMinorityTypeSpec(request.Code);
            var match = await _repository.FindAnyAsync(spec);

            if (match)
            {
                throw new DuplicateCodeException(nameof(MinorityType), request.Code);
            }

            var type = new MinorityType(request.Code, request.Description);

            _repository.Add(type);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return type.Id;
        }
    }
}
