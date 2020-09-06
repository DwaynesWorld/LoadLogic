using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Commands.Vendors
{
    /// <summary>
    /// An immutable command for requesting the deletion of a vendor.
    /// </summary>
    public sealed class DeleteVendor : IRequest
    {
        public DeleteVendor(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Existing vendor identifier.
        /// </summary>
        public long Id { get; }
    }

    internal class DeleteVendorHandler : IRequestHandler<DeleteVendor>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _repository;

        public DeleteVendorHandler(IMediator mediator, ICrudRepository<Vendor> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Unit> Handle(DeleteVendor request, CancellationToken cancellationToken)
        {
            var vendor = await _repository.FindByIdAsync(request.Id);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.Id.ToString());
            }

            _repository.Remove(vendor);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
