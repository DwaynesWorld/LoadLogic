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
            long customerId, string customerName,
            Email customerEmail, PhoneNumber customerPhone,
            string jobName, string jobDescription, Address jobAddress,
            DateTime jobStartDate, DateTime? jobEndDate)
        {
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.CustomerEmail = customerEmail;
            this.CustomerPhone = customerPhone;
            this.JobName = jobName;
            this.JobDescription = jobDescription;
            this.JobAddress = jobAddress;
            this.JobStartDate = jobStartDate;
            this.JobEndDate = jobEndDate;
        }

        public long CustomerId { get; }
        public string CustomerName { get; }
        public Email CustomerEmail { get; }
        public PhoneNumber CustomerPhone { get; }
        public string JobName { get; }
        public string JobDescription { get; }
        public Address JobAddress { get; }
        public DateTime JobStartDate { get; }
        public DateTime? JobEndDate { get; }
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
                orderNo, request.CustomerId,
                request.CustomerName, request.CustomerEmail,
                request.CustomerPhone, request.JobName,
                request.JobDescription, request.JobAddress,
                request.JobStartDate, request.JobEndDate);

            _orderRepository.Add(order);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            await _publishEndpoint.Publish(new OrderConfirmedEvent { UserId = 9999 }, cancellationToken);

            _createOrderCount.Inc();

            return order.Id;
        }
    }
}
