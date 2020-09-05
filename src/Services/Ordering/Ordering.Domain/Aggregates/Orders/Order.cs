using System;
using System.Collections.Generic;
using LoadLogic.Services.Common;
using LoadLogic.Services.Common.Abstractions;
using LoadLogic.Services.Ordering.Domain.Events;

namespace LoadLogic.Services.Ordering.Domain.Aggregates.Orders
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly HashSet<OrderItem> _orderItems = new HashSet<OrderItem>();

        public static Order NewDraft()
        {
            var order = new Order();
            order.IsDraft = true;
            return order;
        }

        public Order(
            int orderNo, long customerId, string customerName,
            Email customerEmail, PhoneNumber customerPhone, string jobName,
            string jobDescription, Address jobAddress, DateTime jobStartDate,
            DateTime? jobEndDate)
        {
            this.OrderNo = orderNo;
            this.OrderStatus = OrderStatus.Confirmed;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.CustomerEmail = customerEmail;
            this.CustomerPhone = customerPhone;
            this.JobName = jobName;
            this.JobDescription = jobDescription;
            this.JobAddress = jobAddress;
            this.JobStartDate = jobStartDate;
            this.JobEndDate = jobEndDate;

            this.AddOrderConfirmedDomainEvent(orderNo, customerId);
        }

        public int OrderNo { get; private set; }
        public OrderStatus? OrderStatus { get; private set; }
        public bool IsDraft { get; set; }

        public long? CustomerId { get; private set; }
        public string? CustomerName { get; private set; }
        public Email? CustomerEmail { get; private set; }
        public PhoneNumber? CustomerPhone { get; private set; }

        public string? JobName { get; private set; }
        public string? JobDescription { get; private set; }
        public Address? JobAddress { get; private set; }
        public DateTime? JobStartDate { get; set; }
        public DateTime? JobEndDate { get; set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;


        private void AddOrderConfirmedDomainEvent(int orderNo, long customerId)
        {
            var domainEvent = new OrderConfirmedDomainEvent(orderNo, customerId);
            this.AddDomainEvent(domainEvent);
        }

        /// <summary>
        /// Used for Persistence and Draft Orders.
        /// </summary>
        private Order() { }
    }
}
