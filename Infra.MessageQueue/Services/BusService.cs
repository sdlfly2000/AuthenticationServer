using Application.Services.Events;
using Application.Services.Events.Messages;
using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using Infra.Core.MessageQueue.Marks;

namespace Infra.MessageQueue.Services
{
    [ServiceLocate(typeof(IBusService))]
    public class BusService : IBusService
    {
        private readonly IBus _eventbus;
        private readonly IAdvancedBus _bus;

        public BusService(IBus eventbus)
        {
            _bus = eventbus.Advanced;
        }

        public async Task publish<TMessage>(TMessage message, string routingKey) where TMessage : BaseMessage
        {
            var queue = await _bus.QueueDeclareAsync(message.GetType().Name);
            var exchange = await _bus.ExchangeDeclareAsync(message.GetType().Name, ExchangeType.Topic);
            var binging = await _bus.BindAsync(exchange, queue, "register");

            var amqpMessage = new Message<TMessage>(message);

            await _bus.PublishAsync(exchange, "register", false, amqpMessage);
        }
    }
}
