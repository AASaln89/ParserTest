using System;
using System.Collections.Generic;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class MaterialDto
    {
        public int MaterialId { get; set; }
        public string? MaterialUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
