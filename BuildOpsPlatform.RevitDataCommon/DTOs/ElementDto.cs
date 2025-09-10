using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class ElementDto
    {
        public int ElementId { get; set; }
        public string? ElementUniqueId { get; set; }
        public string? TypeName { get; set; }
        public int CategoryId { get; set; }
        public int? LevelId { get; set; }
        public int? WorksetId { get; set; }
        public int? PhaseId { get; set; }
        public int? DesignOptionId { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
    }
}
