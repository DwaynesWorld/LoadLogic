using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.Application.Models.Orders
{
    public class OrderSummaryDto
    {
        public long Id { get; set; }
        public int OrderNo { get; set; }
        public OrderType Type { get; set; } = OrderType.Haul;
        public long CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
