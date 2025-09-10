using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class CompanyResponse
    {
        public Guid Id { get; set; }
            public string Name { get; set; }
            public string? Description { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.MinValue;
            public List<MemberDto> Members { get; set; }
            public List<FeatureDto> Features { get; set; }
    }
}
