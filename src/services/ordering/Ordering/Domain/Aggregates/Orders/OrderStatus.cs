using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services.Ordering.Domain.Exceptions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class OrderStatus : Enumeration
    {
        public static readonly OrderStatus Draft = new(0, nameof(Draft).ToLowerInvariant());
        public static readonly OrderStatus Requested = new(1, nameof(Requested).ToLowerInvariant());
        public static readonly OrderStatus Confirmed = new(2, nameof(Confirmed).ToLowerInvariant());
        public static readonly OrderStatus InProgress = new(3, nameof(InProgress).ToLowerInvariant());
        public static readonly OrderStatus Complete = new(4, nameof(Complete).ToLowerInvariant());
        public static readonly OrderStatus Paid = new(5, nameof(Paid).ToLowerInvariant());
        public static readonly OrderStatus Cancelled = new(6, nameof(Cancelled).ToLowerInvariant());

        public OrderStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<OrderStatus> List() =>
            new[] { Draft, Requested, Confirmed, InProgress, Complete, Paid, Cancelled };

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
