namespace Application.Services.Events.Messages
{
    public class UserMessage : BaseMessage
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
