using MessageQueue.RabbitMQ.MessageQueue.Marks;

namespace Infra.Core.MessageQueue.RabbitMQ.Services
{
    public interface IBusService
    {
        public Task Publish<TMessage>(TMessage message, string routingKey) where TMessage: DomainEvent;
    }
}
