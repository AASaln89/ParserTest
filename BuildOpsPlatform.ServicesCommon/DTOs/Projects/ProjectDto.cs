namespace BuildOpsPlatform.ServicesCommon.DTOs.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public Guid CreatorUserId { get; set; }
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int Rating { get; set; }
        public bool IsPublic { get; set; }
    }
}
