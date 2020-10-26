using System.Threading.Tasks;
using LoadLogic.Services.Core.IntegrationEvents;
using MassTransit;

namespace LoadLogic.Services.Dispatching.Application.IntegrationEventConsumers
{
    public class OrderConfirmedIntegrationEventConsumer : IConsumer<OrderConfirmedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<OrderConfirmedIntegrationEvent> context)
        {
            System.Console.WriteLine("Made It");
            System.Console.WriteLine(context.Message.UserId.ToString());
            return Task.CompletedTask;
        }
    }
}
