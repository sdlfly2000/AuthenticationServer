using EasyNetQ;

namespace Infra.Core.MessageQueue.RabbitMQ.MessageQueue.Marks
{
    public interface IAMQPConsumer
    {
        public Task ProcessMessage(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info);
    }
}
