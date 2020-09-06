using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public abstract class Product : Entity
    {
        public long TypeId { get; protected set; }
#nullable disable
        public ProductType Type { get; protected set; }
#nullable enable
        public long? RegionId { get; protected set; }
        public Region? Region { get; protected set; }
    }
}
