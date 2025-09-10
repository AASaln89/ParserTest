namespace BuildOpsPlatform.ServicesCommon.DTOs.Projects
{
    public class UpdateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublic { get; set; }
    }
}
