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
    /// An immutable command message for requesting the update of an company products.
    /// </summary>
    public sealed class UpdateProfileProduct : IRequest
    {
        public UpdateProfileProduct(long id, long productTypeId, long? regionId)
        {
            this.Id = id;
            this.ProductTypeId = productTypeId;
            this.RegionId = regionId;
        }

        /// <summary>
        /// The company product's unique identifier.
        /// </summary>
        [Required]
        public long Id { get; }

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

    internal class UpdateProfileProductHandler : AsyncRequestHandler<UpdateProfileProduct>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly ICrudRepository<ProductType> _productTypeRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public UpdateProfileProductHandler(
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

        protected override async Task Handle(UpdateProfileProduct request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec, Profile.IncludeProducts);
            if (profile == null)
            {
                throw new NotFoundException(nameof(Profile), default);
            }

            var product = profile.FindProductById(request.Id);

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
            await _profileRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
