using System;
using LoadLogic.Services.Ordering.Domain.Events;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Order : Entity
    {
        public Order(
            Guid credentialsCompanyId, Guid businessUnitId,
            int orderNo, long contractorId)
        {
            this.CredentialsCompanyId = credentialsCompanyId;
            this.BusinessUnitId = businessUnitId;
            this.OrderNo = orderNo;
            this.ContractorId = contractorId;
            this.OrderStatus = OrderStatus.Confirmed;

            this.AddOrderConfirmedDomainEvent(
                credentialsCompanyId,
                businessUnitId,
                orderNo,
                contractorId);
        }

        private Order() { }

        public int OrderNo { get; private set; }
        public long ContractorId { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private void AddOrderConfirmedDomainEvent(
            Guid credentialsCompanyId, Guid businessUnitId,
            int orderNo, long contractorId)
        {

            var domainEvent = new OrderConfirmedDomainEvent(
                credentialsCompanyId,
                businessUnitId,
                orderNo,
                contractorId);

            this.AddDomainEvent(domainEvent);
        }
    }
}
