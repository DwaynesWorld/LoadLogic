using System;
using LoadLogic.Services.Ordering.Domain.Aggregates.Orders;

namespace LoadLogic.Services.Ordering.API.Controllers.V1.Models
{
    public class NewRouteLegDto
    {
        public RouteLegType Type { get; set; }
        public int LegOrder { get; set; }
        public Address? Address { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
