using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Route : Entity
    {
        private readonly List<Leg> _legs = new List<Leg>();

        public Route(OrderItem orderItem, List<Leg> legs)
        {
            this.OrderItem = orderItem;
            this.OrderItemId = orderItem.Id;
            _legs = legs;
        }

        public long OrderItemId { get; set; }
        public OrderItem OrderItem { get; }
        public IReadOnlyCollection<Leg> Legs => _legs;


        /// <summary>
        /// The initial departure location.
        /// </summary>
        /// <returns></returns>
        public Point? InitialDepartureLocation()
        {
            if (_legs.Any())
            {
                return _legs.First().LoadLocation;
            }

            return null;
        }

        /// <summary>
        /// The final arrival location.
        /// </summary>
        /// <returns></returns>
        public Point? FinalArrivalLocation()
        {
            if (_legs.Any())
            {
                return _legs.Last().DumpLocation;
            }

            return null;
        }

        /// <summary>
        /// DateTime when load arrives at final destination.
        /// </summary>
        /// <returns></returns>
        public DateTime? FinalArrivalDate()
        {
            if (_legs.Any())
            {
                return _legs.Last().DumpTime;
            }

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
