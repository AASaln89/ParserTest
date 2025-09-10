using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class DesignOptionDto
    {
        public int? DesignOptionId { get; set; }
        public string? DesignOptionUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
}
}
