using Application.Services.Events.Messages;
using Common.Core.DependencyInjection;
using EasyNetQ;
using Infra.Core.MessageQueue.Attributes;
using Infra.Core.MessageQueue.Marks;
using Newtonsoft.Json;

namespace AuthServiceEventServices.Consumers
{
    [AMQPConsumer(ConsumerTypeName = typeof(RegisterUserConsumer), MessageTypeName = typeof(UserMessage), RoutingKeys =["register"])]
    [ServiceLocate(default, ServiceType.Singleton)]
    public class RegisterUserConsumer : IAMQPConsumer
    {
        public async Task ProcessMessage(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info)
        {
            var messagePayload = JsonConvert.DeserializeObject<UserMessage>(body.ToString());
        }
    }
}
