using System;
using NetTopologySuite.Geometries;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Leg : Entity
    {
        public Leg(Route route, Point loadLocation, Address loadAddress, Point dumpLocation, Address dumpAddress, DateTime loadTime, DateTime dumpTime)
        {
            this.Route = route;
            this.RouteId = route.Id;
            this.LoadLocation = loadLocation;
            this.LoadAddress = loadAddress;
            this.DumpLocation = dumpLocation;
            this.DumpAddress = dumpAddress;
            this.LoadTime = loadTime;
            this.DumpTime = dumpTime;
        }

        public long RouteId { get; private set; }
        public Route Route { get; private set; }
        public Point LoadLocation { get; private set; }
        public Address LoadAddress { get; private set; }
        public Point DumpLocation { get; private set; }
        public Address DumpAddress { get; private set; }
        public DateTime LoadTime { get; private set; }
        public DateTime DumpTime { get; private set; }

#nullable disable
        /// <summary>
        /// Needed for Persistence
        /// </summary>
        private Leg() { }
#nullable enable
    }
}
