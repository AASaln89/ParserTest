namespace BuildOpsPlatform.ServicesCommon.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Access { get; set; } = null!;

        public string Refresh { get; set; } = null!;

        public DateTime Expiration { get; set; }
    }
}
