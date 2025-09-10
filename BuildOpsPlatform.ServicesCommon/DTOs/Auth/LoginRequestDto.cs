namespace BuildOpsPlatform.ServicesCommon.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = null!;

        public string? Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
