using System.ComponentModel.DataAnnotations;

using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.MinorityTypes
{
    /// <summary>
    /// An immutable command message for requesting the update of a minority type.
    /// </summary>
    public sealed class UpdateMinorityType : IRequest
    {
        public UpdateMinorityType(long id, string code, string description)
        {
            this.Id = id;
            this.Code = code;
            this.Description = description;
        }

        /// <summary>
        /// The existing minority type's identifier.
        /// </summary>
        [Required]
        public long Id { get; private set; }

        /// <summary>
        /// A unique minority type code.
        /// </summary>
        [Required]
        public string Code { get; private set; } = string.Empty;

        /// <summary>
        /// The minority type's description.
        /// </summary>
        public string Description { get; private set; } = string.Empty;
    }


    internal class UpdateMinorityTypeHandler : IRequestHandler<UpdateMinorityType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<MinorityType> _repository;

        public UpdateMinorityTypeHandler(
            IMediator mediator,
            ICrudRepository<MinorityType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(UpdateMinorityType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(MinorityType), request.Id.ToString());
            }

            if (type.Code.ToUpper() != request.Code.ToUpper())
            {
                var spec = new UniqueMinorityTypeSpec(request.Code);
                var match = await _repository.FindAnyAsync(spec);
                if (match)
                {
                    throw new DuplicateCodeException(nameof(MinorityType), request.Code);
                }
            }

            type.Update(request.Code, request.Description);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
