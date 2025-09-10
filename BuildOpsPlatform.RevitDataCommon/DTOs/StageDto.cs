using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class StageDto
    {
        public int StageId { get; set; }
        public string? StageUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
