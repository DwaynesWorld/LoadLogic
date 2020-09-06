
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
    /// An immutable command message for requesting the creation of vendor products.
    /// </summary>
    public sealed class CreateVendorProduct : IRequest<long>
    {
        public CreateVendorProduct(long vendorId, long productTypeId, long? regionId)
        {
            this.VendorId = vendorId;
            this.ProductTypeId = productTypeId;
            this.RegionId = regionId;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The product type's unique identifier.
        /// </summary>
        [Required]
        public long ProductTypeId { get; }

        /// <summary>
        /// The product region's unique identifier.
        /// </summary>
        public long? RegionId { get; }
    }

    internal class CreateVendorProductHandler : IRequestHandler<CreateVendorProduct, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;
        private readonly ICrudRepository<ProductType> _productTypeRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public CreateVendorProductHandler(
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

        public async Task<long> Handle(CreateVendorProduct request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeProducts);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            var type = await _productTypeRepo.FindByIdAsync(request.ProductTypeId);
            if (type == null)
            {
                throw new NotFoundException(nameof(ProductType), request.ProductTypeId);
            }

            Region? region = null;
            if (request.RegionId.HasValue)
            {
                region = await _regionRepo.FindByIdAsync(request.RegionId.Value);
                if (region == null)
                {
                    throw new NotFoundException(nameof(Region), request.RegionId.Value);
                }
            }

            var product = new VendorProduct(vendor, type, region);
            vendor.AddProduct(product);

            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return product.Id;
        }
    }
}
