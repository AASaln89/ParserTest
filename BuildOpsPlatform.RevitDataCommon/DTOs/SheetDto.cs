using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class SheetDto
    {
        public int SheetId { get; set; }
        public string? SheetUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
