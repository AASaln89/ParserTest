using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
