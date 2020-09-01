using System;

namespace LoadLogic.Services.Core.IntegrationEvents
{
    public class OrderConfirmedIntegrationEvent
    {

        public OrderConfirmedIntegrationEvent(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
