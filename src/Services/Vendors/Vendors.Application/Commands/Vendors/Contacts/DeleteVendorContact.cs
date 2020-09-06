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
    /// An immutable command message for requesting the deletion of a contact.
    /// </summary>
    public sealed class DeleteVendorContact : IRequest
    {
        public DeleteVendorContact(long vendorId, long contactId)
        {
            this.VendorId = vendorId;
            this.ContactId = contactId;
        }

        /// <summary>
        /// The vendor's unique identifier.
        /// </summary>
        [Required]
        public long VendorId { get; }

        /// <summary>
        /// The contact's unique identifier.
        /// </summary>
        [Required]
        public long ContactId { get; }
    }

    internal class DeleteVendorContactHandler : AsyncRequestHandler<DeleteVendorContact>
    {
        private readonly IMediator _mediator;
        private readonly ICrudRepository<Vendor> _repository;

        public DeleteVendorContactHandler(
            IMediator mediator,
            ICrudRepository<Vendor> repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(DeleteVendorContact request, CancellationToken cancellationToken)
        {
            var vendor = await _repository.FindByIdAsync(request.VendorId, Vendor.IncludeVendors);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            vendor.RemoveContact(request.ContactId);
            await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
