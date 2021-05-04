using System.Collections.Generic;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class CreateOrderApiRequest
    {
        public int Type { get; set; }
        public long CustomerId { get; set; }
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public IReadOnlyCollection<CreateOrderLineItemApiRequest> OrderLineItems { get; set; } = new List<CreateOrderLineItemApiRequest>();
    }
}
