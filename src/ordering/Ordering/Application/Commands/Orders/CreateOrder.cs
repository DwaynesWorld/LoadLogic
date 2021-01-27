using System;
using System.Threading;
using System.Threading.Tasks;
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
            string jobName, string jobDescription, Address jobAddress,
            DateTime jobStartDate)
        {
            this.Type = type;
            this.CustomerId = customerId;
            this.CustomerFirstName = customerFirstName;
            this.CustomerLastName = customerLastName;
            this.CustomerEmail = customerEmail;
            this.CustomerPhone = customerPhone;
            this.JobName = jobName;
            this.JobDescription = jobDescription;
            this.JobAddress = jobAddress;
            this.JobStartDate = jobStartDate;
        }

        public OrderType Type { get; }
        public long CustomerId { get; }
        public string CustomerFirstName { get; }
        public string CustomerLastName { get; }
        public Email CustomerEmail { get; }
        public PhoneNumber CustomerPhone { get; }
        public string JobName { get; }
        public string JobDescription { get; }
        public Address JobAddress { get; }
        public DateTime JobStartDate { get; }
    }

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private static readonly Counter _createOrderCount = Metrics.CreateCounter("ordering_create_order_total", "Number of orders created.");
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
        {
            _publishEndpoint = publishEndpoint;
            _orderRepository = orderRepository;
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderNo = await _orderRepository.GetNextOrderNo(cancellationToken);

            var order = new Order(
                orderNo, request.Type, request.CustomerId,
                request.CustomerFirstName, request.CustomerLastName,
                request.CustomerEmail, request.CustomerPhone,
                request.JobName, request.JobDescription,
                request.JobAddress, request.JobStartDate);

            _orderRepository.Add(order);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            // await _publishEndpoint.Publish(new OrderConfirmedEvent { UserId = 9999 }, cancellationToken);

            _createOrderCount.Inc();

            return order.Id;
        }
    }
}
