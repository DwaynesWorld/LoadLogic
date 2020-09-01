using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Ordering.Domain.Events;
using MediatR;

namespace LoadLogic.Services.Ordering.Application.DomainEventHandlers
{
    public class OrderConfirmedDomainEventHandler : INotificationHandler<OrderConfirmedDomainEvent>
    {
        public OrderConfirmedDomainEventHandler()
        {
        }

        public Task Handle(OrderConfirmedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Send email, etc.
            System.Console.WriteLine("Handling OrderConfirmedDomainEvent");
            return Task.CompletedTask;
        }
    }
}
