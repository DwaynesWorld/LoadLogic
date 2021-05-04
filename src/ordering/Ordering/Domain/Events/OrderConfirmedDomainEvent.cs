using System;
using MediatR;

namespace LoadLogic.Services.Ordering.Domain.Events
{
    public class OrderConfirmedDomainEvent : INotification
    {
        public OrderConfirmedDomainEvent(int orderNo, long customerId)
        {
            this.OrderNo = orderNo;
            this.CustomerId = customerId;
        }

        public long CredentialsCompanyId { get; }
        public long BusinessUnitId { get; }
        public int OrderNo { get; }
        public long CustomerId { get; }
    }
}
