using System;
using System.Collections.Generic;
using LoadLogic.Services.Abstractions;
using LoadLogic.Services.Ordering.Domain.Events;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly HashSet<OrderLineItem> _orderLineItems = new();

        public Order(
            int orderNo, OrderType type, long customerId, string customerFirstName,
            string customerLastName, Email customerEmail, PhoneNumber customerPhone)
        {
            this.OrderNo = orderNo;
            this.Type = type;
            this.OrderStatus = OrderStatus.Draft;
            this.CustomerId = customerId;
            this.CustomerFirstName = customerFirstName;
            this.CustomerLastName = customerLastName;
            this.CustomerEmail = customerEmail;
            this.CustomerPhone = customerPhone;

            this.AddOrderCreatedDomainEvent(orderNo, customerId);
        }

        public int OrderNo { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public OrderType Type { get; private set; }

        public long CustomerId { get; private set; }
        public string CustomerFirstName { get; private set; }
        public string CustomerLastName { get; private set; }
        public Email CustomerEmail { get; private set; }
        public PhoneNumber CustomerPhone { get; private set; }

        public IReadOnlyCollection<OrderLineItem> OrderLineItems => _orderLineItems;
        public bool IsDraft => this.OrderStatus == OrderStatus.Draft;

        public void AddOrderLineItem(OrderLineItem item)
        {
            _orderLineItems.Add(item);
        }

        private void AddOrderCreatedDomainEvent(int orderNo, long customerId)
        {
            var domainEvent = new OrderCreatedDomainEvent(orderNo, customerId);
            this.AddDomainEvent(domainEvent);
        }

#nullable disable
        /// <summary>
        /// Used for Persistence and Draft Orders.
        /// </summary>
        private Order() { }
    }
#nullable enable
}
