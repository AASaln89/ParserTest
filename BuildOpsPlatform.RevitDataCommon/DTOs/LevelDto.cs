using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class LevelDto
    {
        public int LevelId { get; set; }
        public string? LevelUniqueId { get; set; }
        public string? Name { get; set; }
        public string? ElevationValue { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
