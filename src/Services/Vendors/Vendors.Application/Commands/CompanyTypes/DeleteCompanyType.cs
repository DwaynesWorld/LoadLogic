
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.CompanyTypes
{
    /// <summary>
    /// An immutable command message for requesting the deletion of a company type.
    /// </summary>
    public sealed class DeleteCompanyType : IRequest
    {
        public DeleteCompanyType(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// The existing company type's identifier.
        /// </summary>
        [Required]
        public long Id { get; }
    }


    internal class DeleteCompanyTypeHandler : IRequestHandler<DeleteCompanyType>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<CompanyType> _repository;

        public DeleteCompanyTypeHandler(
            IMediator mediator,
            ICrudRepository<CompanyType> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteCompanyType request, CancellationToken cancellationToken)
        {
            var type = await _repository.FindByIdAsync(request.Id);
            if (type == null)
            {
                throw new NotFoundException(nameof(CompanyType), request.Id.ToString());
            }

            try
            {
                _repository.Remove(type);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new EntityDeletionException(nameof(CompanyType), request.Id.ToString());
                }

                throw;
            }

            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
