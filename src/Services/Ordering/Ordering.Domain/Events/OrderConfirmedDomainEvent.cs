using System;
using MediatR;

namespace LoadLogic.Services.Ordering.Domain.Events
{
    public class OrderConfirmedDomainEvent : INotification
    {
        public OrderConfirmedDomainEvent(int orderNo, long customerId)
        {
            this.OrderNo = orderNo;
            this.customerId = customerId;
        }

        public Guid CredentialsCompanyId { get; }
        public Guid BusinessUnitId { get; }
        public int OrderNo { get; }
        public long customerId { get; }
    }
}
