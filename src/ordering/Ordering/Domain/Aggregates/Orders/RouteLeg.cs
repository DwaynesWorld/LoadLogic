using System;
using NetTopologySuite.Geometries;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class RouteLeg : Entity
    {
        public RouteLeg(Route route, RouteLegType type, Point location, Address address, DateTime timestamp)
        {
            this.Route = route;
            this.RouteId = route.Id;
            this.Type = type;
            this.Location = location;
            this.Address = address;
            this.Timestamp = timestamp;
        }

        public long RouteId { get; private set; }
        public Route Route { get; private set; }
        public RouteLegType Type { get; private set; }
        public Point Location { get; private set; }
        public Address Address { get; private set; }
        public DateTime Timestamp { get; private set; }

#nullable disable
        /// <summary>
        /// Needed for Persistence
        /// </summary>
        private RouteLeg() { }
#nullable enable
    }
}
