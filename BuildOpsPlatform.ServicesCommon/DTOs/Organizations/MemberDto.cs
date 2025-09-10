using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class MemberDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public GlobalUserRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
