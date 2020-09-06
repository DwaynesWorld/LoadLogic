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
    /// An immutable command message for requesting an update of an vendor minority status.
    /// </summary>
    public sealed class UpdateVendorMinorityStatus : IRequest
    {
        public UpdateVendorMinorityStatus(
            long vendorId,
            long statusId,
            long minorityTypeId,
            string certificationNumber,
            decimal percent)
        {
            this.VendorId = vendorId;
            this.StatusId = statusId;
            this.MinorityTypeId = minorityTypeId;
            this.CertificationNumber = certificationNumber;
            this.Percent = percent;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The vendor minority status's unique identifier.
        /// </summary>
        [Required]
        public long StatusId { get; }

        /// <summary>
        /// The minority status type's unique identifier.
        /// </summary>
        [Required]
        public long MinorityTypeId { get; }

        /// <summary>
        /// The vendor's minority status certification number.
        /// </summary>
        public string CertificationNumber { get; } = string.Empty;

        /// <summary>
        /// The vendor's minority percentage.
        /// </summary>
        public decimal Percent { get; }
    }

    internal class UpdateVendorMinorityStatusHandler : AsyncRequestHandler<UpdateVendorMinorityStatus>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;
        private readonly ICrudRepository<MinorityType> _minorityTypeRepo;

        public UpdateVendorMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo,
            ICrudRepository<MinorityType> minorityTypeRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
            _minorityTypeRepo = minorityTypeRepo ?? throw new ArgumentNullException(nameof(minorityTypeRepo));
        }

        protected override async Task Handle(UpdateVendorMinorityStatus request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeStatuses);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            var status = vendor.FindMinorityStatusById(request.StatusId);

            var type = status.Type;
            if (type == null || type.Id != request.MinorityTypeId)
            {
                type = await _minorityTypeRepo.FindByIdAsync(request.MinorityTypeId);
                if (type == null)
                {
                    throw new NotFoundException(nameof(MinorityType), request.MinorityTypeId);
                }
            }

            status.Update(type, request.CertificationNumber, request.Percent);
            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
