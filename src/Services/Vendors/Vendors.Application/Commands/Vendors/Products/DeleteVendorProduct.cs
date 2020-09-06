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
    /// An immutable request for requesting the deletion of a vendor's product.
    /// </summary>
    public sealed class DeleteVendorProduct : IRequest
    {
        public DeleteVendorProduct(long vendorId, long productId)
        {
            this.VendorId = vendorId;
            this.ProductId = productId;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The product's unique identifier.
        /// </summary>
        [Required]
        public long ProductId { get; }
    }

    internal class DeleteVendorProductHandler : AsyncRequestHandler<DeleteVendorProduct>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;

        public DeleteVendorProductHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
        }

        protected override async Task Handle(DeleteVendorProduct request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeProducts);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            vendor.RemoveProduct(request.ProductId);
            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
