using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Auth
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        public GlobalUserRole Role { get; set; }
    }
}
