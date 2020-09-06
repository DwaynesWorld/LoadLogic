using System;
using System.Collections.Generic;
using MediatR;

namespace LoadLogic.Services
{
    public class Entity
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();

        public long Id { get; protected set; }

        // public long IdentityCompanyId { get; protected set; }
        // public long BusinessUnitId { get; protected set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
        public void RemoveDomainEvent(INotification eventItem) => _domainEvents.Remove(eventItem);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
