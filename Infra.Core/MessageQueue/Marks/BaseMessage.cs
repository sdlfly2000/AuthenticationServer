namespace Infra.Core.MessageQueue.Marks
{
    public abstract class BaseMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
