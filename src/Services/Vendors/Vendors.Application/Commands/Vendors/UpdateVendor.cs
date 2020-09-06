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
    /// An immutable command message for requesting the update of a vendor.
    /// </summary>
    public sealed class UpdateVendor : IRequest
    {
        public UpdateVendor(
            long id, string code, string name, long? typeId,
            Address? primaryAddress, Address? alternateAddress,
            PhoneNumber? phoneNumber, PhoneNumber? faxNumber, string webAddress,
            long? regionId, CommunicationMethod communicationMethod,
            bool isBonded, decimal bondRate, string note)
        {
            this.Id = id;
            this.Code = code;
            this.Name = name;
            this.TypeId = typeId;
            this.PrimaryAddress = primaryAddress;
            this.AlternateAddress = alternateAddress;
            this.PhoneNumber = phoneNumber;
            this.FaxNumber = faxNumber;
            this.WebAddress = webAddress;
            this.RegionId = regionId;
            this.CommunicationMethod = communicationMethod;
            this.IsBonded = isBonded;
            this.BondRate = bondRate;
            this.Note = note;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// The vendor's unique code.
        /// </summary>
        [Required]
        public string Code { get; } = string.Empty;

        /// <summary>
        /// The vendor's name.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// The private vendor type unique identifier. 
        /// </summary>
        public long? TypeId { get; }

        /// <summary>
        /// The vendor's primary address.
        /// </summary>
        public Address? PrimaryAddress { get; }

        /// <summary>
        /// The vendor's alternate address.
        /// </summary>
        public Address? AlternateAddress { get; }

        public PhoneNumber? PhoneNumber { get; }

        public PhoneNumber? FaxNumber { get; }

        /// <summary>
        /// The vendor's website address.
        /// </summary>
        public string WebAddress { get; } = string.Empty;

        /// <summary>
        /// The unique identifier of the region the vendor operates in.
        /// </summary>
        public long? RegionId { get; }

        /// <summary>
        /// The best form of communication of contacting this vendor.
        /// </summary>
        public CommunicationMethod CommunicationMethod { get; }

        /// <summary>
        /// A flag indicating if this vendor is bonded.
        /// </summary>
        public bool IsBonded { get; }

        /// <summary>
        /// The rate at which this vendor is bonded.
        /// </summary>
        public decimal BondRate { get; }

        /// <summary>
        /// Notes about this vendor.
        /// </summary>
        public string Note { get; } = string.Empty;
    }

    internal class UpdateVendorHandler : IRequestHandler<UpdateVendor>
    {

        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _vendorRepo;
        private readonly ICrudRepository<CompanyType> _companyTypeRepo;
        private readonly ICrudRepository<Region> _regionRepo;

        public UpdateVendorHandler(
            IMediator mediator,
            ICrudRepository<Vendor> vendorRepository,
            ICrudRepository<CompanyType> companyTypeRepository,
            ICrudRepository<Region> regionRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vendorRepo = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
            _companyTypeRepo = companyTypeRepository ?? throw new ArgumentNullException(nameof(companyTypeRepository));
            _regionRepo = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));
        }

        public async Task<Unit> Handle(UpdateVendor request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepo.FindByIdAsync(
                request.Id,
                Vendor.IncludeCompanyType,
                Vendor.IncludeRegion);

            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.Id);
            }

            if (vendor.Code.ToUpper() != request.Code.ToUpper())
            {
                var spec = new UniqueVendorSpec(request.Code);
                var match = await _vendorRepo.FindAnyAsync(spec);
                if (match)
                {
                    throw new DuplicateCodeException(nameof(Vendor), request.Code);
                }
            }

            var type = vendor.Type;
            if (request.TypeId.HasValue && (type == null || type.Id != request.TypeId))
            {
                type = await _companyTypeRepo.FindByIdAsync(request.TypeId.Value);
                if (type == null)
                {
                    throw new NotFoundException(nameof(CompanyType), request.TypeId.Value);
                }
            }

            var region = vendor.Region;
            if (request.RegionId.HasValue && (region == null || region.Id != request.RegionId))
            {
                region = await _regionRepo.FindByIdAsync(request.RegionId.Value);
                if (region == null)
                {
                    throw new NotFoundException(nameof(Region), request.RegionId.Value);
                }
            }

            vendor.Update(
                request.Code, request.Name, type,
                request.PrimaryAddress, request.AlternateAddress,
                request.PhoneNumber, request.FaxNumber,
                request.WebAddress, region,
                request.CommunicationMethod, request.IsBonded,
                request.BondRate, request.Note);

            await _vendorRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
