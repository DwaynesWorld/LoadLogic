using System;
using MediatR;

namespace LoadLogic.Services.Ordering.Domain.Events
{
    public class OrderConfirmedDomainEvent : INotification
    {
        public OrderConfirmedDomainEvent(
            Guid credentialsCompanyId, Guid businessUnitId,
            int orderNo, long contractorId)
        {
            this.CredentialsCompanyId = credentialsCompanyId;
            this.BusinessUnitId = businessUnitId;
            this.OrderNo = orderNo;
            this.ContractorId = contractorId;
        }

        public Guid CredentialsCompanyId { get; }
        public Guid BusinessUnitId { get; }
        public int OrderNo { get; }
        public long ContractorId { get; }
    }
}
