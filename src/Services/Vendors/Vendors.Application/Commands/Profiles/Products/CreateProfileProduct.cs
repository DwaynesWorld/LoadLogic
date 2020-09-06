using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Profiles
{
    /// <summary>
    /// An immutable command message for requesting the creation of company products.
    /// </summary>
    public sealed class CreateProfileProduct : IRequest<long>
    {
        public CreateProfileProduct(long productTypeId, long? regionId)
        {
            this.ProductTypeId = productTypeId;
            this.RegionId = regionId;
        }

        /// <summary>
        /// The product type's unique identifier.
        /// </summary>
        [Required]
        public long ProductTypeId { get; private set; }

        /// <summary>
        /// The product region's unique identifier.
        /// </summary>
        public long? RegionId { get; private set; }
    }

    internal class CreateProfileProductHandler : IRequestHandler<CreateProfileProduct, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly ICrudRepository<ProductType> _productTypeRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public CreateProfileProductHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo,
            ICrudRepository<ProductType> productTypeRepo,
            ICrudRepository<Region> regionRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
            _productTypeRepo = productTypeRepo ?? throw new ArgumentNullException(nameof(productTypeRepo));
            _regionRepo = regionRepo ?? throw new ArgumentNullException(nameof(regionRepo));
        }

        public async Task<long> Handle(CreateProfileProduct request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeProducts);
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), default);
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

            var product = new ProfileProduct(profile, type, region);
            profile.AddProduct(product);

            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return product.Id;
        }
    }
}
