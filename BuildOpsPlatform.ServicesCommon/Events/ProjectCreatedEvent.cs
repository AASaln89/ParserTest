namespace BuildOpsPlatform.ServicesCommon.Events
{
    public class ProjectCreatedEvent
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool IsPublic { get; set; }
        public string? ImageUrl { get; set; }
        public string? TemplateId { get; set; }
    }
}
