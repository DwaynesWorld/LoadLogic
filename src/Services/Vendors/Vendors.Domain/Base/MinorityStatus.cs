using System;

namespace LoadLogic.Services.Vendors.Domain
{
    public abstract class MinorityStatus : Entity
    {
        public long TypeId { get; protected set; }
#nullable disable warnings
        public MinorityType Type { get; protected set; }
#nullable enable warnings
        public string CertificationNumber { get; protected set; } = string.Empty;
        public decimal Percent { get; protected set; }
    }
}
