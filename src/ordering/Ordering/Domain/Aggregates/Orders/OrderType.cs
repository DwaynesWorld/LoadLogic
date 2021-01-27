using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.Domain.Exceptions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class OrderType : Enumeration
    {
        public static OrderType Haul = new OrderType(1, nameof(Haul).ToLowerInvariant());
        public static OrderType OnSiteLoadDump = new OrderType(2, nameof(OnSiteLoadDump).ToLowerInvariant());
        public static OrderType MultiSiteLoadDump = new OrderType(3, nameof(MultiSiteLoadDump).ToLowerInvariant());

        public OrderType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<OrderType> List() => new[] { Haul, OnSiteLoadDump, MultiSiteLoadDump };

        public static OrderType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                var values = string.Join(",", List().Select(s => s.Name));
                throw new OrderingDomainException($"Possible values for OrderActivity: {values}");
            }

            return state;
        }

        public static OrderType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                var values = string.Join(",", List().Select(s => s.Name));
                throw new OrderingDomainException($"Possible values for OrderActivity: {values}");
            }

            return state;
        }
    }
}
