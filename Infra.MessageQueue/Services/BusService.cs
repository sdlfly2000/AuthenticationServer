using Application.Services.Events;
using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using MessageQueue.RabbitMQ.MessageQueue.Marks;
using Microsoft.Extensions.Logging;
using Infra.Core.LogTrace;

namespace MessageQueue.RabbitMQ.Services
{
    [ServiceLocate(typeof(IBusService))]
    public class BusService : IBusService
    {
        private readonly IAdvancedBus _bus;
        private readonly ILogger<BusService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BusService(IBus eventbus, ILogger<BusService> logger, IServiceProvider serviceProvider)
        {
            _bus = eventbus.Advanced;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        public async Task Publish<TMessage>(TMessage message, string routingKey) where TMessage : DomainEvent
        {
            var exchange = await CreateExchangeIfNotExist(message.GetType().Name);

            var amqpMessage = new Message<TMessage>(message);

            await _bus.PublishAsync(exchange, routingKey, false, amqpMessage);

            _logger.LogInformation($"{nameof(BusService)}: Message published to exchange {{Exchange}} with routing key {{RoutingKey}}, amqpMessageId: {{amqpMessageId}}", exchange.Name, routingKey, amqpMessage.Body.Id);
        }

        private async Task<Exchange> CreateExchangeIfNotExist(string exchangeName)
        {
            return await _bus.ExchangeDeclareAsync(exchangeName, (configuration) =>
            {
                configuration
                    .AsDurable(true)
                    .WithType(ExchangeType.Topic);
            });
        }
    }
}
