using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class ViewDto
    {
        public int ViewId { get; set; }
        public string? ViewUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
