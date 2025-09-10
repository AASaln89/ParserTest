using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Projects
{
    public class AddProjectMemberDto
    {
        public Guid ProjectId { get; set; }
        public string Email { get; set; } = string.Empty;
        public GlobalUserRole? Role { get; set; }
    }
}
