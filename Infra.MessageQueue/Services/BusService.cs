using Application.Services.Events;
using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using MessageQueue.RabbitMQ.MessageQueue.Marks;
using Microsoft.Extensions.Logging;

namespace MessageQueue.RabbitMQ.Services
{
    [ServiceLocate(typeof(IBusService))]
    public class BusService : IBusService
    {
        private readonly IAdvancedBus _bus;
        private readonly ILogger<BusService> _logger;

        public BusService(IBus eventbus, ILogger<BusService> logger)
        {
            _bus = eventbus.Advanced;
            _logger = logger;
        }

        public async Task publish<TMessage>(TMessage message, string routingKey) where TMessage : BaseMessage
        {
            var exchange = new Exchange(message.GetType().Name, ExchangeType.Topic);

            var amqpMessage = new Message<TMessage>(message);

            await _bus.PublishAsync(exchange, "register", false, amqpMessage);

            _logger.LogInformation($"{nameof(BusService)}: Message published to exchange {{Exchange}} with routing key {{RoutingKey}}, amqpMessageId: {{amqpMessageId}}", exchange.Name, routingKey, amqpMessage.Body.Id);
        }
    }
}
