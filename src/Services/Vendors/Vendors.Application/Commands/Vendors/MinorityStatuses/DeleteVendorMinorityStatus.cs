using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Vendors
{
    /// <summary>
    /// An immutable command message for requesting the deletion of an vendor's minority status.
    /// </summary>
    public sealed class DeleteVendorMinorityStatus : IRequest
    {
        public DeleteVendorMinorityStatus(long vendorId, long statusId)
        {
            this.VendorId = vendorId;
            this.StatusId = statusId;
        }

        /// <summary>
        /// /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The minority status's unique identifier.
        /// </summary>
        [Required]
        public long StatusId { get; }
    }


    internal class DeleteVendorMinorityStatusHandler : AsyncRequestHandler<DeleteVendorMinorityStatus>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;

        public DeleteVendorMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
        }

        protected override async Task Handle(DeleteVendorMinorityStatus request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeStatuses);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            vendor.RemoveMinorityStatus(request.StatusId);
            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
