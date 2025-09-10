using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class ErrorDto
    {
        public Guid ErrorId { get; set; }
        public string? Message { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
    }
}
