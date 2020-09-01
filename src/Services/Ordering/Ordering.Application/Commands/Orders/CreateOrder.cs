using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Core.IntegrationEvents;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using MassTransit;
using MediatR;

namespace LoadLogic.Services.Ordering.Application.Commands.Orders
{
    public sealed class CreateOrder : IRequest<long>
    {
        public CreateOrder(int orderNo, long contractorId)
        {
            this.OrderNo = orderNo;
            this.ContractorId = contractorId;
        }

        public int OrderNo { get; set; }
        public long ContractorId { get; set; }
    }

    internal class CreateOrderHandler : IRequestHandler<CreateOrder, long>
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateOrderHandler(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        public Task<long> Handle(CreateOrder request, CancellationToken cancellationToken)
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), request.OrderNo, request.ContractorId);

            // TODO Order persistence
            // orderRepo.Add(order);
            // orderRepo.SaveChangesAsync();

            _publishEndpoint.Publish(new OrderConfirmedIntegrationEvent(Guid.Empty));
            foreach (var @event in order.DomainEvents)
            {
                _mediator.Publish(@event);
            }

            return Task.FromResult((long)1000);
        }
    }
}
