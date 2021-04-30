using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Ordering.Application.Models.Orders;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using MassTransit;
using MediatR;
using Prometheus;

namespace LoadLogic.Services.Ordering.Application.Commands.Orders
{
    public sealed class CreateOrderCommand : IRequest<long>
    {
        public CreateOrderCommand(
            OrderType type, long customerId, string customerFirstName,
            string customerLastName, Email customerEmail, PhoneNumber customerPhone,
            IReadOnlyCollection<CreateOrderLineItemDto> orderLineItems)
        {
            this.Type = type;
            this.CustomerId = customerId;
            this.CustomerFirstName = customerFirstName;
            this.CustomerLastName = customerLastName;
            this.CustomerEmail = customerEmail;
            this.CustomerPhone = customerPhone;
            this.OrderLineItems = orderLineItems;
        }

        public OrderType Type { get; }
        public long CustomerId { get; }
        public string CustomerFirstName { get; }
        public string CustomerLastName { get; }
        public Email CustomerEmail { get; }
        public PhoneNumber CustomerPhone { get; }
        public IReadOnlyCollection<CreateOrderLineItemDto> OrderLineItems { get; }
    }

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private static readonly Counter _createOrderCount = Metrics.CreateCounter("ordering_create_order_total", "Number of orders created.");
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderNo = await _orderRepository.GetNextOrderNo(cancellationToken);

            var order = new Order(
                orderNo, request.Type, request.CustomerId,
                request.CustomerFirstName, request.CustomerLastName,
                request.CustomerEmail, request.CustomerPhone);

            await _orderRepository.Add(order, cancellationToken);

            foreach (var item in request.OrderLineItems)
            {
                var lineItem = CreateOrderLineItem(order, item);
                order.AddOrderLineItem(lineItem);
            }


            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            // await _publishEndpoint.Publish(new OrderConfirmedEvent { UserId = 9999 }, cancellationToken);

            _createOrderCount.Inc();

            return order.Id;
        }

        private static OrderLineItem CreateOrderLineItem(Order order, CreateOrderLineItemDto item)
        {
            var lineItem = new OrderLineItem(
                order, item.MaterialName,
                item.MaterialUnit, item.MaterialQuantity,
                "TEMP", 1, "TEMP", (decimal)100.01);

            if (item.Route is not null)
            {
                var route = new Route(lineItem);

                foreach (var leg in item.Route.RouteLegs)
                {
                    var routeLeg = new RouteLeg(
                        route,
                        leg.Type,
                        leg.Address ?? new Address(),
                        leg.Timestamp);

                    route.AddRouteLeg(routeLeg);
                }
            }

            return lineItem;
        }
    }
}
