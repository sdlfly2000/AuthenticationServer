using MessageQueue.RabbitMQ.MessageQueue.Marks;

namespace Application.Services.Events
{
    public interface IBusService
    {
        public Task Publish<TMessage>(TMessage message, string routingKey) where TMessage: BaseMessage;
    }
}
