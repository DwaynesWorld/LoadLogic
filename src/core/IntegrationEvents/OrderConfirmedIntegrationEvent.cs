using System;

namespace LoadLogic.Services.Core.IntegrationEvents
{
    public class OrderConfirmedIntegrationEvent
    {

        public OrderConfirmedIntegrationEvent(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; private set; }
    }
}
