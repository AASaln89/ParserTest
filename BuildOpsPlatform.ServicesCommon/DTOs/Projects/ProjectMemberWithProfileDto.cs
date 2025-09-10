using BuildOpsPlatform.ServicesCommon.DTOs.Profile;
using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Projects
{
    public class ProjectMemberWithProfileDto
    {
        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public ProjectUserRole? Role { get; set; }

        public UserProfileDto? UserProfile { get; set; } = null!;
    }

}
