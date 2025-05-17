using Application.Services.Events;
using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using MessageQueue.RabbitMQ.MessageQueue.Marks;

namespace MessageQueue.RabbitMQ.Services
{
    [ServiceLocate(typeof(IBusService))]
    public class BusService : IBusService
    {
        private readonly IAdvancedBus _bus;

        public BusService(IBus eventbus)
        {
            _bus = eventbus.Advanced;
        }

        public async Task publish<TMessage>(TMessage message, string routingKey) where TMessage : BaseMessage
        {
            //var exchangee = await _bus.ExchangeDeclareAsync(message.GetType().Name, ExchangeType.Topic);
            var exchange = new Exchange(message.GetType().Name, ExchangeType.Topic);

            var amqpMessage = new Message<TMessage>(message);

            await _bus.PublishAsync(exchange, "register", false, amqpMessage);
        }
    }
}
