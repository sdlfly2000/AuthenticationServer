using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using Infra.Core.LogTrace;
using MessageQueue.RabbitMQ.MessageQueue.Marks;

namespace Infra.Core.MessageQueue.RabbitMQ.Services
{
    [ServiceLocate(typeof(IBusService))]
    public class BusService : IBusService
    {
        private readonly IAdvancedBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public BusService(IBus eventbus, IServiceProvider serviceProvider)
        {
            _bus = eventbus.Advanced;
            _serviceProvider = serviceProvider;
        }

        [LogTrace]
        public async Task Publish<TMessage>(TMessage message, string routingKey) where TMessage : DomainEvent
        {
            var exchange = await CreateExchangeIfNotExist(message.GetType().Name);

            var amqpMessage = new Message<TMessage>(message);

            await _bus.PublishAsync(exchange, routingKey, false, amqpMessage);
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
