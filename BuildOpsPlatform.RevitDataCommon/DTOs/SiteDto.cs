using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class SiteDto
    {
        public int SiteId { get; set; }
        public string? SiteUniqueId { get; set; }
        public string? Name { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? BasePointElevetionValue { get; set; }
        public string? BasePointEastWest { get; set; }
        public string? BasePointNorthSouth { get; set; }
        public string? BasePointAngle { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
