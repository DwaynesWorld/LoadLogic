using System.Threading;
using System.Threading.Tasks;
using LoadLogic.Services.Exceptions;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;
using MassTransit;
using MediatR;

namespace Ordering.Application.Commands.Orders
{
    public class CreateOrderLineItem : IRequest<long>
    {
        public CreateOrderLineItem(
            long orderId, Route route,
            string materialName, string materialUnit,
            double materialQuantity, string truckType, int truckQuantity,
            string chargeType, decimal chargeRate)
        {
            this.OrderId = orderId;
            this.Route = route;
            this.MaterialName = materialName;
            this.MaterialUnit = materialUnit;
            this.MaterialQuantity = materialQuantity;
            this.TruckType = truckType;
            this.TruckQuantity = truckQuantity;
            this.ChargeType = chargeType;
            this.ChargeRate = chargeRate;
        }

        public long OrderId { get; }
        public Route Route { get; }
        public long MaterialId { get; }
        public string MaterialName { get; } = string.Empty;
        public string MaterialUnit { get; } = string.Empty;
        public double MaterialQuantity { get; }
        public string TruckType { get; } = string.Empty;
        public int TruckQuantity { get; }
        public string ChargeType { get; } = string.Empty;
        public decimal ChargeRate { get; }
    }

    internal class CreateOrderLineItemCommandHandler : IRequestHandler<CreateOrderLineItem, long>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderLineItemCommandHandler(IPublishEndpoint publishEndpoint, IOrderRepository orderRepository)
        {
            _publishEndpoint = publishEndpoint;
            _orderRepository = orderRepository;
        }

        public async Task<long> Handle(CreateOrderLineItem request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Order), request.OrderId);
            }

            var lineItem = new OrderLineItem(
                order, request.Route, request.MaterialName,
                request.MaterialUnit, request.MaterialQuantity,
                request.TruckType, request.TruckQuantity, request.ChargeType, request.ChargeRate);

            order.AddOrderLineItem(lineItem);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return lineItem.Id;
        }
    }
}
