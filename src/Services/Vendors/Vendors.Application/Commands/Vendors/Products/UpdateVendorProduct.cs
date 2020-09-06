using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Vendors
{
    /// <summary>
    /// An immutable command message for requesting the update of an vendor products.
    /// </summary>
    public sealed class UpdateVendorProduct : IRequest
    {
        public UpdateVendorProduct(long vendorId, long productId, long productTypeId, long? regionId)
        {
            this.VendorId = vendorId;
            this.ProductId = productId;
            this.ProductTypeId = productTypeId;
            this.RegionId = regionId;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The vendor product's unique identifier.
        /// </summary>
        [Required]
        public long ProductId { get; }

        /// <summary>
        /// The product types's unique identifier.
        /// </summary>
        [Required]
        public long ProductTypeId { get; }

        /// <summary>
        /// The product region's unique identifier.
        /// </summary>
        public long? RegionId { get; }
    }

    internal class UpdateVendorProductHandler : AsyncRequestHandler<UpdateVendorProduct>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;
        private readonly ICrudRepository<ProductType> _productTypeRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public UpdateVendorProductHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo,
            ICrudRepository<ProductType> productTypeRepo,
            ICrudRepository<Region> regionRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
            _productTypeRepo = productTypeRepo ?? throw new ArgumentNullException(nameof(productTypeRepo));
            _regionRepo = regionRepo ?? throw new ArgumentNullException(nameof(regionRepo));
        }

        protected override async Task Handle(UpdateVendorProduct request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeProducts);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            var product = vendor.FindProductById(request.ProductId);

            var type = product.Type;
            if (request.ProductTypeId != type.Id)
            {
                var updatedType = await _productTypeRepo.FindByIdAsync(request.ProductTypeId);
                if (updatedType == null)
                {
                    throw new NotFoundException(nameof(ProductType), request.ProductTypeId);
                }

                type = updatedType;
            }

            var region = product.Region;
            if (request.RegionId.HasValue && (region == null || region.Id != request.RegionId.Value))
            {
                region = await _regionRepo.FindByIdAsync(request.RegionId.Value);
                if (region == null)
                {
                    throw new NotFoundException(nameof(Region), request.RegionId.Value);
                }
            }

            product.Update(type, region);
            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
