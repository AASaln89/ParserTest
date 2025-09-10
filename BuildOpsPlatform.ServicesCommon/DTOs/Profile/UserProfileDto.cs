namespace BuildOpsPlatform.ServicesCommon.DTOs.Profile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

        public string? Position { get; set; }

        public string? Bio { get; set; }

        public string? AvatarBase64 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    }
}
