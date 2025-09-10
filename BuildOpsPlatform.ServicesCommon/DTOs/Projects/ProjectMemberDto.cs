using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Projects
{
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }

        public ProjectUserRole? Role { get; set; } = ProjectUserRole.Guest;

        public Guid ProjectId { get; set; }
    }
}
