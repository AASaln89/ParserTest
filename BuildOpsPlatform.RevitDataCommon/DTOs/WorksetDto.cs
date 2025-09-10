using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class WorksetDto
    {
        public int WorksetId { get; set; }
        public string? WorksetUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
