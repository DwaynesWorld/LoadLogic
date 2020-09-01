using System;
using System.Collections.Generic;
using MediatR;

namespace LoadLogic.Services.Ordering.Domain
{
    public class Entity
    {
        private List<INotification> _domainEvents;

        public long Id { get; protected set; }
        public Guid CredentialsCompanyId { get; protected set; }
        public Guid BusinessUnitId { get; protected set; }
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();


        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
