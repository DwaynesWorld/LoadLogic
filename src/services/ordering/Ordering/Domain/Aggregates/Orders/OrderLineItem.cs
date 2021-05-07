using LoadLogic.Services;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class OrderLineItem : Entity
    {
        public OrderLineItem(
            Order order,
            string materialName, string materialUnit,
            double materialQuantity, string truckType, int truckQuantity,
            string chargeType, decimal chargeRate)
        {
            this.Order = order;
            this.OrderId = order.Id;
            this.MaterialName = materialName;
            this.MaterialUnit = materialUnit;
            this.MaterialQuantity = materialQuantity;
            this.TruckType = truckType;
            this.TruckQuantity = truckQuantity;
            this.ChargeType = chargeType;
            this.ChargeRate = chargeRate;
        }

        public long OrderId { get; private set; }
        public Order Order { get; private set; }
        public Route? Route { get; set; }
        public long MaterialId { get; private set; }
        public string MaterialName { get; private set; } = string.Empty;
        public string MaterialUnit { get; private set; } = string.Empty;
        public double MaterialQuantity { get; private set; }
        public string TruckType { get; private set; } = string.Empty;
        public int TruckQuantity { get; private set; }
        public string ChargeType { get; private set; } = string.Empty;
        public decimal ChargeRate { get; private set; }

        public void SetRoute(Route route)
        {
            this.Route = route;
        }

#nullable disable
        /// <summary>
        /// Needed for Persistence
        /// </summary>
        private OrderLineItem() { }
#nullable enable
    }
}
