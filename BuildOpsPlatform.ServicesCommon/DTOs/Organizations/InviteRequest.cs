using BuildOpsPlatform.ServicesCommon.Enums;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class InviteRequest
    {
        public string Email { get; set; }
        public GlobalUserRole Role { get; set; }
    }
}
