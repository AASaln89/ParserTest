using System;
namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class GridDto
    {
        public int GridId { get; set; }
        public string? GridUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
