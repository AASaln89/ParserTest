namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Site
    {
        public int? SiteId { get; set; }
        public string? SiteUniqueId { get; set; }
        public string? Name { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? BasePointElevetionValue { get; set; }
        public string? BasePointEastWest { get; set; }
        public string? BasePointNorthSouth { get; set; }
        public string? BasePointAngle { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }
    }
}
