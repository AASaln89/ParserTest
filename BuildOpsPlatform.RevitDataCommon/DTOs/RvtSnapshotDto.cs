using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class RvtSnapshotDto
    {
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }
        public string DocumentId { get; set; } = null!;
    }
}
