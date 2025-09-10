namespace BuildOpsPlatform.ServicesCommon.Events
{
    public class UserCreatedEvent
    {
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    }
}
