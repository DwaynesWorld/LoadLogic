using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public abstract class Company : Entity
    {
        public string Name { get; protected set; } = string.Empty;
        public Address? PrimaryAddress { get; protected set; }
        public Address? AlternateAddress { get; protected set; }
        public PhoneNumber? PhoneNumber { get; protected set; }
        public PhoneNumber? FaxNumber { get; protected set; }
        public string WebAddress { get; protected set; } = string.Empty;
        public long? RegionId { get; protected set; }
        public Region? Region { get; protected set; }
        public CommunicationMethod CommunicationMethod { get; protected set; }
    }
}
