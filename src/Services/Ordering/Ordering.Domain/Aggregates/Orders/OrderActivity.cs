using System;
using System.Collections.Generic;
using System.Linq;
using LoadLogic.Services;
using LoadLogic.Services.Ordering.Domain.Exceptions;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class OrderActivity : Enumeration
    {
        public static OrderActivity HaulAway = new OrderActivity(1, nameof(HaulAway).ToLowerInvariant());
        public static OrderActivity OnSiteLoadDump = new OrderActivity(2, nameof(OnSiteLoadDump).ToLowerInvariant());
        public static OrderActivity MultiSiteLoadDump = new OrderActivity(3, nameof(MultiSiteLoadDump).ToLowerInvariant());

        public OrderActivity(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<OrderActivity> List() => new[] { HaulAway, OnSiteLoadDump, MultiSiteLoadDump };

        public static OrderActivity FromName(string name)
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

        public static OrderActivity From(int id)
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
