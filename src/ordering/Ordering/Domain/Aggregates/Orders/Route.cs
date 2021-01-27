using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Route : Entity
    {
        private readonly List<RouteLeg> _routeLegs = new();

        public Route(OrderLineItem orderItem, List<RouteLeg> legs)
        {
            this.OrderItem = orderItem;
            this.OrderItemId = orderItem.Id;
            _routeLegs = legs;
        }

        public long OrderItemId { get; set; }
        public OrderLineItem OrderItem { get; }
        public IReadOnlyCollection<RouteLeg> RouteLegs => _routeLegs;


        /// <summary>
        /// The initial location.
        /// </summary>
        /// <returns></returns>
        public Point? InitialLocation()
        {
            if (_routeLegs.Any())
                return _routeLegs.First().Location;

            return null;
        }

        /// <summary>
        /// The final arrival location.
        /// </summary>
        /// <returns></returns>
        public Point? FinalArrivalLocation()
        {
            if (_routeLegs.Any())
                return _routeLegs.Last().Location;

            return null;
        }

        /// <summary>
        /// DateTime when load arrives at final destination.
        /// </summary>
        /// <returns></returns>
        public DateTime? FinalArrivalDate()
        {
            if (_routeLegs.Any())
                return _routeLegs.Last().Timestamp;

            return null;
        }

#nullable disable
        /// <summary>
        /// Needed for Persistence
        /// </summary>
        private Route() { }
#nullable enable
    }
}
