using LoadLogic.Services;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.Application.Models.Orders
{
    public class OrderLineItemDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public Route? Route { get; set; }
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialUnit { get; set; } = string.Empty;
        public double MaterialQuantity { get; set; }
        public string TruckType { get; set; } = string.Empty;
        public int TruckQuantity { get; set; }
        public string ChargeType { get; set; } = string.Empty;
        public decimal ChargeRate { get; set; }
    }
}
