using MessageQueue.RabbitMQ.MessageQueue.Marks;

namespace Application.Services.Events.Messages
{
    public class UserRegisterdEvent : DomainEvent
    {
        public DateTime CreatedOnUTC { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }

        public static string RoutingKeyRegister => "register";
    }
}
