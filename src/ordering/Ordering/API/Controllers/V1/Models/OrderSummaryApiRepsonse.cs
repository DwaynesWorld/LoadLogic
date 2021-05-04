
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class OrderSummaryApiResponse
    {
        public long Id { get; set; }
        public int OrderNo { get; set; }
        public int Type { get; set; }
        public long CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
