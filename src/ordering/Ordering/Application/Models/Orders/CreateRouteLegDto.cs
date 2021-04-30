using System;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.Application.Models.Orders
{
    public class CreateRouteLegDto
    {
        public RouteLegType Type { get; set; }
        public int LegOrder { get; set; }
        public Address? Address { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
