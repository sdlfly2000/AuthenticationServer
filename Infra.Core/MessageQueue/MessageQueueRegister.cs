using Common.Core.DependencyInjection;
using EasyNetQ;
using EasyNetQ.Topology;
using Infra.Core.MessageQueue.Attributes;
using Infra.Core.MessageQueue.Marks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infra.Core.MessageQueue
{
    [ServiceLocate(default)]
    public class MessageQueueRegister
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAdvancedBus _advancedBus;

        public MessageQueueRegister(IServiceProvider serviceProvider, IBus eventBus)
        {
            _serviceProvider = serviceProvider;
            _advancedBus = eventBus.Advanced;
        }

        public void Register(params string[] domains)
        {
            var asms = domains.Select(domain => Assembly.Load(domain)).ToList();
            var impls = asms.SelectMany(asm => asm.GetExportedTypes().Where(t => !t.IsInterface).ToList()).ToList();

            foreach (var impl in impls)
            {
                var consumer = impl.GetCustomAttribute(typeof(AMQPConsumerAttribute));
                if (consumer != null)
                {
                    var consumerTypeName = (consumer as AMQPConsumerAttribute).ConsumerTypeName;
                    var messageTypeName = (consumer as AMQPConsumerAttribute).MessageTypeName;
                    var routingKeys = (consumer as AMQPConsumerAttribute).RoutingKeys;

                    var queueName = $"{messageTypeName.Name}-{consumerTypeName.Name}-({string.Join('.', routingKeys)})";
                    var queue = _advancedBus.QueueDeclare(queueName);
                    var exchange = _advancedBus.ExchangeDeclare(messageTypeName.Name, ExchangeType.Topic);
                    foreach (var routingKey in routingKeys)
                    {
                        _advancedBus.Bind(exchange, queue, routingKey);
                    }

                    var consumerService = _serviceProvider.GetRequiredService(consumerTypeName) as IAMQPConsumer;
                    _advancedBus.Consume(queue, consumerService.ProcessMessage);
                }
            }
        }
    }
}
