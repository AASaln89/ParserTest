using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public string? ScheduleUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
