namespace Application.Services.Events.Messages
{
    public abstract class BaseMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
