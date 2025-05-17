namespace MessageQueue.RabbitMQ.MessageQueue.Attributes
{
    public class AMQPConsumerAttribute : Attribute
    {
        public Type MessageTypeName { get; set; }
        public Type ConsumerTypeName { get; set; }
        public string[] RoutingKeys { get; set; }
    }
}
