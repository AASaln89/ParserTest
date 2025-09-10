using BuildOpsPlatform.ServicesCommon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class InviteResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public GlobalUserRole Role { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
