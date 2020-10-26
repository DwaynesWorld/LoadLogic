using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.Domain.Exceptions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class OrderStatus : Enumeration
    {
        public static OrderStatus Requested = new OrderStatus(1, nameof(Requested).ToLowerInvariant());
        public static OrderStatus Confirmed = new OrderStatus(2, nameof(Confirmed).ToLowerInvariant());
        public static OrderStatus InProgress = new OrderStatus(3, nameof(InProgress).ToLowerInvariant());
        public static OrderStatus Complete = new OrderStatus(4, nameof(InProgress).ToLowerInvariant());
        public static OrderStatus Paid = new OrderStatus(5, nameof(Paid).ToLowerInvariant());
        public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

        public OrderStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<OrderStatus> List() =>
            new[] { Requested, Confirmed, InProgress, Complete, Paid, Cancelled };

        public static OrderStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                var values = string.Join(",", List().Select(s => s.Name));
                throw new OrderingDomainException($"Possible values for OrderStatus: {values}");
            }

            return state;
        }

        public static OrderStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                var values = string.Join(",", List().Select(s => s.Name));
                throw new OrderingDomainException($"Possible values for OrderStatus: {values}");
            }

            return state;
        }
    }
}
