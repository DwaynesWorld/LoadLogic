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
    /// An immutable command message for requesting the creation of an vendor minority status.
    /// </summary>
    public sealed class CreateVendorMinorityStatus : IRequest<long>
    {
        public CreateVendorMinorityStatus(
            long vendorId,
            long minorityTypeId,
            string certificationNumber,
            decimal percent)
        {
            this.VendorId = vendorId;
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


    internal class CreateVendorMinorityStatusHandler : IRequestHandler<CreateVendorMinorityStatus, long>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;
        private readonly ICrudRepository<MinorityType> _minorityTypeRepo;

        public CreateVendorMinorityStatusHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepo,
            ICrudRepository<MinorityType> minorityTypeRepo)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepo ?? throw new ArgumentNullException(nameof(vendorRepo));
            _minorityTypeRepo = minorityTypeRepo ?? throw new ArgumentNullException(nameof(minorityTypeRepo));
        }

        public async Task<long> Handle(CreateVendorMinorityStatus request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(request.VendorId, Vendor.IncludeStatuses);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            var type = await _minorityTypeRepo.FindByIdAsync(request.MinorityTypeId);
            if (type == null)
            {
                throw new NotFoundException(nameof(MinorityType), request.MinorityTypeId);
            }

            var status = new VendorMinorityStatus(
                vendor,
                type,
                request.CertificationNumber,
                request.Percent);

            vendor.AddMinorityStatus(status);
            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return status.Id;
        }
    }
}
