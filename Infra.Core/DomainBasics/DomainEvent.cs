namespace MessageQueue.RabbitMQ.MessageQueue.Marks
{
    public abstract class DomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
